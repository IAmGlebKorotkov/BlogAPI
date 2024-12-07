using BlogAPI.DbContext;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly TagContext _tagContext;

    public TagController(TagContext tagContext)
    {
        _tagContext = tagContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
    {
        var tags = await _tagContext.Tags.ToListAsync();
        return Ok(tags);
    }
}