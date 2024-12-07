using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BlogAPI.Service;

public class AuthService
{
    private readonly UserManager<UserDto> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<UserDto> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> AuthenticateAsync(LoginCredentials credentials)
    {
        var user = await _userManager.FindByEmailAsync(credentials.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, credentials.Password))
        {
            var token = GenerateJwtToken(user);
            return token;
        }

        return null;
    }

    public string GenerateJwtToken(UserDto user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(1);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}