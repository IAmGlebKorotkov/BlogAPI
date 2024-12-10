
using BlogAPI.DbContext;
using BlogAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly UserManager<UserDto> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<UserDto> userManager, ApplicationDbContext context, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }
    public async Task<IdentityResult> RegisterUserAsync(UserRegisterModel model)
    {
        // Хеширование пароля с использованием BCrypt
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            PhoneNumber = model.PhoneNumber,
            CreateTime = DateTime.UtcNow,
            PasswordHash = hashedPassword // Сохраняем хешированный пароль
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"User registered successfully with email: {model.Email}");

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateUserProfileAsync(string userId, UserEditModel model)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        user.Email = model.Email;
        user.FullName = model.FullName;
        user.BirthDate = model.BirthDate;
        user.Gender = model.Gender;
        user.PhoneNumber = model.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        return result;
    }

    public async Task<UserDto> FindUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}