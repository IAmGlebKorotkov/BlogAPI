using BlogAPI.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Models;

public class UserDto : IdentityUser
{
    public DateTime CreateTime { get; set; }
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public GenderEnum Gender { get; set; }
    public string PhoneNumber { get; set; }
}