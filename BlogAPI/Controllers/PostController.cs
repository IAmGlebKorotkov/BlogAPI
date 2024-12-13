using System.Security.Claims;
using BlogAPI.DbContext;
using BlogAPI.Models;
using BlogAPI.Models.Author;
using BlogAPI.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly PostContext _postContext;
    private readonly CommunityContext _communityContext;
    private readonly ILogger<PostController> _logger;
    private readonly AuthorContext _authorContext;
    private readonly ApplicationDbContext _userContext;

    public PostController(PostContext postContext, 
        CommunityContext communityContext, 
        ILogger<PostController> logger,
        AuthorContext authorContext,
        ApplicationDbContext userContext
        )
    {
        _postContext = postContext;
        _communityContext = communityContext;
        _authorContext = authorContext;
        _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PostPageListDTO>> GetPosts(
        [FromQuery] string authorFullName = null,
        [FromQuery] int? minReadingTime = null,
        [FromQuery] int? maxReadingTime = null,
        [FromQuery] PostSorting sorting = PostSorting.CreateDesc,
        [FromQuery] bool onlySubscribed = false,
        [FromQuery] int page = 1,
        [FromQuery] int size = 5
    )
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (onlySubscribed && userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }
            
            var query = _postContext.Posts.AsQueryable();
            
            if (!string.IsNullOrEmpty(authorFullName))
            {
                query = query.Where(p => p.Author.Contains(authorFullName));
            }
            
            if (minReadingTime.HasValue)
            {
                query = query.Where(p => p.ReadingTime >= minReadingTime.Value);
            }

            if (maxReadingTime.HasValue)
            {
                query = query.Where(p => p.ReadingTime <= maxReadingTime.Value);
            }
            
            if (onlySubscribed)
            {
                var subscribedCommunities = await _communityContext.CommunityUsers
                    .Where(cu => cu.UserId == userId)
                    .Select(cu => cu.CommunityId)
                    .ToListAsync();

                query = query.Where(p => subscribedCommunities.Contains(p.CommunityId));
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
            _logger.LogError(ex, "An error occurred while retrieving posts.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        try
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
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
                CommunityId = null, 
                CommunityName = null,
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
            _logger.LogError(ex, "An error occurred while creating a post.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        try
        {
            var post = await _postContext.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Post not found" });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                post.HasLike = await _postContext.UserLikes
                    .AnyAsync(ul => ul.PostId == id && ul.UserId == userId);
            }

            return Ok(post);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the post.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }
    
    [Authorize]
    [HttpPost("{postId}/like")]
    public async Task<IActionResult> LikePost(Guid postId)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            // Проверяем, существует ли пост
            var post = await _postContext.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Post not found" });
            }

            // Проверяем, поставил ли пользователь уже лайк
            var existingLike = await _postContext.UserLikes
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.PostId == postId);

            if (existingLike != null)
            {
                return BadRequest(new Response { Status = "Error", Message = "User already liked this post" });
            }

            // Добавляем лайк
            var userLike = new UserLike
            {
                UserId = userId,
                PostId = postId
            };

            _postContext.UserLikes.Add(userLike);
            await _postContext.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Post liked successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while liking the post.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }
    
   
    [Authorize]
    [HttpDelete("{postId}/like")]
    public async Task<IActionResult> UnlikePost(Guid postId)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            // Проверяем, существует ли пост
            var post = await _postContext.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Post not found" });
            }

            // Проверяем, поставил ли пользователь лайк
            var existingLike = await _postContext.UserLikes
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.PostId == postId);

            if (existingLike == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "User has not liked this post" });
            }

            // Удаляем лайк
            _postContext.UserLikes.Remove(existingLike);
            await _postContext.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Post unliked successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while unliking the post.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }
}