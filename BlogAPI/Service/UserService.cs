using BlogAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Service;

public class UserService
{
    private readonly UserManager<UserDto> _userManager;

    public UserService(UserManager<UserDto> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserRegisterModel model)
    {
        var user = new UserDto
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            PhoneNumber = model.PhoneNumber,
            CreateTime = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        return result;
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
        return await _userManager.FindByEmailAsync(email);
    }
}