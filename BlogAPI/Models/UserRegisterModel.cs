using System.ComponentModel.DataAnnotations;
using BlogAPI.Models.Enums;

namespace BlogAPI.Models;

public class UserRegisterModel
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public DateTime BirthDate { get; set; }

    [Required]
    public GenderEnum Gender { get; set; }

    public string PhoneNumber { get; set; }
}