using BlogAPI.DbContext;
using BlogAPI.Filters;
using BlogAPI.Models;
using BlogAPI.Models.Community;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly CommunityContext _communityContext;

    public CommunityController(CommunityContext communityContext)
    {
        _communityContext = communityContext;
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
            var community = await _communityContext.Communities
                .Include(c => c.Administrators)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (community == null)
            {
                return NotFound(new Response
                {
                    Status = "Error",
                    Message = $"Community with id {id} not found."
                });
            }

            var communityFullDto = new CommunityFullDto
            {
                Id = community.Id,
                CreateTime = community.CreateTime,
                Name = community.Name,
                Description = community.Description,
                IsClosed = community.IsClosed,
                SubscribersCount = community.SubscribersCount,
                Administrators = community.Administrators
            };

            return Ok(communityFullDto);
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
}