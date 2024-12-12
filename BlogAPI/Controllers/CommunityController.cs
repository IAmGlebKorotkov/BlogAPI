using System.Security.Claims;
using BlogAPI.DbContext;
using BlogAPI.Models;
using BlogAPI.Models.Community;
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
    private readonly ILogger<CommunityController> _logger;

    public CommunityController(CommunityContext communityContext, ApplicationDbContext userContext, ILogger<CommunityController> logger)
    {
        _communityContext = communityContext;
        _userContext = userContext;
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
            // Логирование ошибки (например, в консоль или файл)
            Console.WriteLine($"Error: {ex.Message}");

            // Возвращаем модель Response с кодом состояния 500
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
            // Найти сообщество по id
            var community = await _communityContext.Communities
                .FirstOrDefaultAsync(c => c.Id == id);

            if (community == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

            // Найти все записи в user_community, связанные с этим community_id
            var userCommunityEntries = await _communityContext.CommunityUsers
                .Where(uc => uc.CommunityId == id)
                .ToListAsync();

            // Получить список user_id из найденных записей
            var userIds = userCommunityEntries.Select(uc => uc.UserId).ToList();

            // Найти пользователей в таблице AspNetUsers по их user_id
            var users = await _userContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            // Сформировать CommunityFullDto
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
    
    [Authorize] // Эндпоинт доступен только для авторизованных пользователей
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<CommunityUserDto>>> GetMyCommunities()
    {
        try
        {
            // Получаем идентификатор текущего пользователя из токена
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            // Ищем все записи в таблице user_community, где user_id равен идентификатору текущего пользователя
            var userCommunities = await _communityContext.CommunityUsers
                .Where(uc => uc.UserId == userId)
                .ToListAsync();

            // Возвращаем список моделей UserCommunity
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
    [Authorize] // Эндпоинт доступен только для авторизованных пользователей
    [HttpPost("{id}/subscribe")]
    public async Task<IActionResult> SubscribeToCommunity(string id)
    {
        try
        {
            // Получаем идентификатор текущего пользователя из токена
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            // Проверяем, существует ли сообщество с указанным id
            var communityExists = await _communityContext.Communities.AnyAsync(c => c.Id == id);
            if (!communityExists)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

            // Проверяем, подписан ли пользователь уже на это сообщество
            var isAlreadySubscribed = await _communityContext.CommunityUsers
                .AnyAsync(uc => uc.UserId == userId && uc.CommunityId == id);

            if (isAlreadySubscribed)
            {
                return BadRequest(new Response { Status = "Error", Message = "User is already subscribed to this community" });
            }

            // Создаем новую запись в таблице user_community
            var newSubscription = new CommunityUserDto
            {
                UserId = userId,
                CommunityId = id,
                Role = "Subscriber" // Устанавливаем роль "Subscriber"
            };

            _communityContext.CommunityUsers.Add(newSubscription);
            await _communityContext.SaveChangesAsync();

            // Возвращаем успешный ответ
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
    [Authorize] // Эндпоинт доступен только для авторизованных пользователей
    [HttpGet("{id}/role")]
    public async Task<IActionResult> GetUserRoleInCommunity(string id)
    {
        try
        {
            // Получаем идентификатор текущего пользователя из токена
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            // Проверяем, существует ли сообщество с указанным id
            var communityExists = await _communityContext.Communities.AnyAsync(c => c.Id == id);
            if (!communityExists)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

            // Ищем роль пользователя в указанном сообществе
            var userRole = await _communityContext.CommunityUsers
                .Where(uc => uc.UserId == userId && uc.CommunityId == id)
                .Select(uc => uc.Role)
                .FirstOrDefaultAsync();

            if (userRole == null)
            {
                return NotFound(new Response { Status = "Error", Message = "User is not a member of this community" });
            }

            // Возвращаем роль пользователя
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
    [Authorize] // Эндпоинт доступен только для авторизованных пользователей
    [HttpDelete("{id}/unsubscribe")]
    public async Task<IActionResult> UnsubscribeFromCommunity(string id)
    {
        try
        {
            // Получаем идентификатор текущего пользователя из токена
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "User is not authenticated" });
            }

            // Проверяем, существует ли сообщество с указанным id
            var communityExists = await _communityContext.Communities.AnyAsync(c => c.Id == id);
            if (!communityExists)
            {
                return NotFound(new Response { Status = "Error", Message = "Community not found" });
            }

            // Ищем запись о подписке пользователя в указанном сообществе
            var subscription = await _communityContext.CommunityUsers
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CommunityId == id);

            if (subscription == null)
            {
                return NotFound(new Response { Status = "Error", Message = "User is not a member of this community" });
            }

            // Удаляем запись о подписке
            _communityContext.CommunityUsers.Remove(subscription);
            await _communityContext.SaveChangesAsync();

            // Возвращаем успешный ответ
            return Ok(new Response { Status = "Success", Message = "User unsubscribed from the community successfully" });
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
}
