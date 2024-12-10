using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BlogAPI.DbContext;

namespace BlogAPI.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> AuthenticateAsync(LoginCredentials credentials)
    {
        // Логирование начала аутентификации
        _logger.LogInformation($"Starting authentication for email: {credentials.Email}");

        // Поиск пользователя по email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == credentials.Email);
        if (user == null)
        {
            _logger.LogWarning($"User with email {credentials.Email} not found.");
            return null;
        }

        // Проверка пароля с использованием BCrypt
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(credentials.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            _logger.LogWarning($"Invalid password for user with email: {credentials.Email}");
            return null;
        }

        // Генерация JWT-токена
        var token = GenerateJwtToken(user);

        // Логирование успешного входа
        _logger.LogInformation($"User with email {credentials.Email} authenticated successfully.");

        return token;
    }

    public string GenerateJwtToken(UserDto user)
    {
        // Создание claims для токена
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Получение ключа из конфигурации
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Создание токена
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}