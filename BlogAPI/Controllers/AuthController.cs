/*using BlogAPI.Models;
using BlogAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _authService.AuthenticateAsync(credentials);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { token });
    }
}*/