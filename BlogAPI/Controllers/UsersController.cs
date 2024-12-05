using System.Security.Claims;
using BlogAPI.Models;
using BlogAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthService _authService;
    private readonly UserManager<UserDto> _userManager;

    public UsersController(UserService userService, AuthService authService, UserManager<UserDto> userManager)
    {
        _userService = userService;
        _authService = authService;
        _userManager = userManager;
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
            return Ok(new { message = "User registered successfully" });
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

        var token = await _authService.AuthenticateAsync(credentials);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { token });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Проверяем, авторизован ли пользователь
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized(new { message = "User is not authenticated" });
        }

        // Выход пользователя обычно заключается в удалении токена доступа из клиентского приложения
        // Сервер не может принудительно отозвать токен, если он уже выдан
        // В данном примере мы просто ничего не делаем, так как не используем хранилище токенов.
        return Ok(new { message = "User logged out successfully" });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        // Получаем идентификатор пользователя из токена
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { message = "User is not authenticated" });
        }

        // Получаем пользователя по идентификатору
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Возвращаем информацию о пользователе
        return Ok(new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.FullName,
            user.BirthDate,
            user.Gender,
            user.PhoneNumber
        });
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserEditModel model)
    {
        // Получаем идентификатор пользователя из токена
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { message = "User is not authenticated" });
        }

        // Обновляем профиль пользователя
        var result = await _userService.UpdateUserProfileAsync(userId, model);

        if (result.Succeeded)
        {
            return Ok(new { message = "Profile updated successfully" });
        }

        return BadRequest(result.Errors);
    }
}