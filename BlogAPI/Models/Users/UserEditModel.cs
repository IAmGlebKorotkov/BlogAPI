using System.ComponentModel.DataAnnotations;
using BlogAPI.Models.Enums;

namespace BlogAPI.Models;

public class UserEditModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public string FullName { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required]
    public GenderEnum Gender { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
}