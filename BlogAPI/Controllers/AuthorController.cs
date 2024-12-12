using BlogAPI.DbContext;
using BlogAPI.Models;
using BlogAPI.Models.Author;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly AuthorContext _authorContext;
    private readonly ILogger<AuthorController> _logger;

    public AuthorController(AuthorContext authorContext, ILogger<AuthorController> logger)
    {
        _authorContext = authorContext;
        _logger = logger;
    }


    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        try
        {

            var authors = await _authorContext.Authors.ToListAsync();

            var authorList = authors.Select(author => new AuthorDto
            {
                FullName = author.FullName,
                BirthDate = author.BirthDate,
                Gender = author.Gender,
                Posts = author.Posts,
                Likes = author.Likes,
                Created = author.Created
            }).ToList();

            return Ok(authorList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the list of authors.");
            return StatusCode(500, new Response
            {
                Status = "Error",
                Message = "An error occurred while processing your request."
            });
        }
    }
}