using System.Security.Claims;
using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthService _authService;
    private readonly UserManager<UserDto> _userManager;
    private readonly ILogger<UsersController> _logger;

    public UsersController(UserService userService, AuthService authService, UserManager<UserDto> userManager, ILogger<UsersController> logger)
    {
        _userService = userService;
        _authService = authService;
        _userManager = userManager;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.RegisterUserAsync(model);

        if (result.Succeeded)
        {

            _logger.LogInformation($"Trying to find user by email: {model.Email}");

            await Task.Delay(500);
            
            var user = await _userService.FindUserByEmailAsync(model.Email);

            if (user == null)
            {
                _logger.LogError($"User not found after registration: {model.Email}");
                return BadRequest("User not found after registration.");
            }


            var token = _authService.GenerateJwtToken(user);

            return Ok(new { message = "User registered successfully", token });
        }

        return BadRequest(result.Errors);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation($"Starting login for email: {credentials.Email}");
        
        var token = await _authService.AuthenticateAsync(credentials);

        if (token == null)
        {
            _logger.LogWarning($"Login failed for email: {credentials.Email}");
            return Unauthorized();
        }
        
        _logger.LogInformation($"Login successful for email: {credentials.Email}");

        return Ok(new { token });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { message = "User is not authenticated" });
        }

        var result = await _authService.LogoutAsync(userId);

        if (result)
        {
            return Ok(new { message = "User logged out successfully" });
        }

        return BadRequest(new { message = "Failed to log out user" });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { message = "User is not authenticated" });
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        var userProfile = new UserProfileDto
        {
            Id = user.Id,
            CreateTime = user.CreateTime,
            FullName = user.FullName,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        return Ok(userProfile);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserEditModel model)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { message = "User is not authenticated" });
        }
        
        var result = await _userService.UpdateUserProfileAsync(userId, model);

        if (result.Succeeded)
        {
            return Ok(new { message = "Profile updated successfully" });
        }

        return BadRequest(result.Errors);
    }
}