using System.Security.Claims;
using BlogAPI.DbContext;
using BlogAPI.Models;
using BlogAPI.Models.Author;
using BlogAPI.Models.Community;
using BlogAPI.Models.Enums;
using BlogAPI.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly CommunityContext _communityContext;
    private readonly ApplicationDbContext _userContext;
    private readonly PostContext _postContext;
    private readonly AuthorContext _authorContext;
    private readonly ILogger<CommunityController> _logger;

    public CommunityController(
        CommunityContext communityContext,
        ApplicationDbContext userContext,
        PostContext postContext,
        AuthorContext authorContext,
        ILogger<CommunityController> logger)
    {
        _communityContext = communityContext;
        _userContext = userContext;
        _postContext = postContext;
        _authorContext = authorContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommunityDto>>> GetCommunities()
    {
        try
        {
            var communities = await _communityContext.Communities.ToListAsync();
            return Ok(communities);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            var response = new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            };
            return StatusCode(500, response);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommunityFullDto>> GetCommunityById(string id)
    {
        try
        {
            var community = await _communityContext.Communities.FirstOrDefaultAsync(c => c.Id == id);
            if (community == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

            var userCommunityEntries = await _communityContext.CommunityUsers
                .Where(uc => uc.CommunityId == id)
                .ToListAsync();

            var userIds = userCommunityEntries.Select(uc => uc.UserId).ToList();

            var users = await _userContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            var communityFullDto = new CommunityFullDto
            {
                Id = community.Id,
                CreateTime = community.CreateTime,
                Name = community.Name,
                Description = community.Description,
                IsClosed = community.IsClosed,
                SubscribersCount = community.SubscribersCount,
                Administrators = users.Select(u => new UserProfileDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FullName = u.FullName,
                    BirthDate = u.BirthDate,
                    Gender = u.Gender,
                    PhoneNumber = u.PhoneNumber,
                    CreateTime = u.CreateTime
                }).ToList()
            };

            return Ok(communityFullDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving community by id.");
            var response = new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            };
            return StatusCode(500, response);
        }
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<CommunityUserDto>>> GetMyCommunities()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            var userCommunities = await _communityContext.CommunityUsers
                .Where(uc => uc.UserId == userId)
                .ToListAsync();

            return Ok(userCommunities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user's communities.");
            var response = new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            };
            return StatusCode(500, response);
        }
    }

    [Authorize]
    [HttpPost("{id}/subscribe")]
    public async Task<IActionResult> SubscribeToCommunity(string id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            var communityExists = await _communityContext.Communities.AnyAsync(c => c.Id == id);
            if (!communityExists)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

            var isAlreadySubscribed = await _communityContext.CommunityUsers
                .AnyAsync(uc => uc.UserId == userId && uc.CommunityId == id);

            if (isAlreadySubscribed)
            {
                return BadRequest(new Response
                    { Status = "Error", Message = "User is already subscribed to this community" });
            }

            var newSubscription = new CommunityUserDto
            {
                UserId = userId,
                CommunityId = id,
                Role = "Subscriber"
            };

            _communityContext.CommunityUsers.Add(newSubscription);
            await _communityContext.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "User subscribed to the community successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while subscribing to the community.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }

    [Authorize]
    [HttpGet("{id}/role")]
    public async Task<IActionResult> GetUserRoleInCommunity(string id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            var communityExists = await _communityContext.Communities.AnyAsync(c => c.Id == id);
            if (!communityExists)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

            var userRole = await _communityContext.CommunityUsers
                .Where(uc => uc.UserId == userId && uc.CommunityId == id)
                .Select(uc => uc.Role)
                .FirstOrDefaultAsync();

            if (userRole == null)
            {
                return NotFound(new Response { Status = "Error", Message = "User is not a member of this community" });
            }

            return Ok(new { Role = userRole });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user role in community.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }

    [Authorize]
    [HttpDelete("{id}/unsubscribe")]
    public async Task<IActionResult> UnsubscribeFromCommunity(string id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            var communityExists = await _communityContext.Communities.AnyAsync(c => c.Id == id);
            if (!communityExists)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

            var subscription = await _communityContext.CommunityUsers
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CommunityId == id);

            if (subscription == null)
            {
                return NotFound(new Response { Status = "Error", Message = "User is not a member of this community" });
            }

            _communityContext.CommunityUsers.Remove(subscription);
            await _communityContext.SaveChangesAsync();

            return Ok(new Response
                { Status = "Success", Message = "User unsubscribed from the community successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while unsubscribing from the community.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }

    [Authorize]
    [HttpPost("{id}/post")]
    public async Task<IActionResult> CreatePostInCommunity(string id, [FromBody] CreatePostDto createPostDto)
    {
        try
        {
       
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

           
            var community = await _communityContext.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

         
            var author = await _authorContext.Authors.FirstOrDefaultAsync(a => a.id_user == userId);
            if (author == null)
            {
           
                var user = await _userContext.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new Response { Status = "Error", Message = "User not found" });
                }

                author = new AuthorFULLDto
                {
                    Id = Guid.NewGuid(),
                    id_user = userId,
                    FullName = user.FullName,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    Created = DateTime.UtcNow,
                    Posts = 0,
                    Likes = 0
                };

                _authorContext.Authors.Add(author);
                await _authorContext.SaveChangesAsync();
            }
            
            var post = new PostDto
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.UtcNow,
                Title = createPostDto.Title,
                Description = createPostDto.Description,
                ReadingTime = createPostDto.ReadingTime,
                Image = createPostDto.Image,
                AddressId = createPostDto.AddressId,
                AuthorId = author.Id,
                Author = author.FullName,
                CommunityId = community.Id,
                CommunityName = community.Name,
                TagPosts = createPostDto.Tags,
                Likes = 0,
                HasLike = false,
                CommentsCount = 0
            };

            _postContext.Posts.Add(post);
            await _postContext.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Post created successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a post in the community.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }

    [HttpGet("{id}/post")]
    public async Task<ActionResult<PostPageListDTO>> GetCommunityPosts(
        string id,
        [FromQuery] List<string> tags = null, 
        [FromQuery] PostSorting sorting = PostSorting.CreateDesc, 
        [FromQuery] int page = 1, 
        [FromQuery] int size = 5 
    )
    {
        try
        {

            var community = await _communityContext.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

    
            var query = _postContext.Posts
                .Where(p => p.CommunityId == id);


            if (tags != null && tags.Any())
            {
                query = query.Where(p => tags.Contains(p.TagPosts));
            }


            switch (sorting)
            {
                case PostSorting.CreateDesc:
                    query = query.OrderByDescending(p => p.CreateTime);
                    break;
                case PostSorting.CreateAsc:
                    query = query.OrderBy(p => p.CreateTime);
                    break;
                case PostSorting.LikeDesc:
                    query = query.OrderByDescending(p => p.Likes);
                    break;
                case PostSorting.LikeAsc:
                    query = query.OrderBy(p => p.Likes);
                    break;
                default:
                    query = query.OrderByDescending(p =>
                        p.CreateTime);
                    break;
            }

            // Пагинация
            var totalItems = await query.CountAsync(); 
            var totalPages = (int)Math.Ceiling(totalItems / (double)size); 

            if (page > totalPages)
            {
                page = totalPages;
            }

            var posts = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            
            var response = new PostPageListDTO
            {
                Posts = posts,
                Pagination = new PageInfoModel
                {
                    Size = size,
                    Count = totalItems,
                    Current = page
                }
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving posts for the community.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }
}