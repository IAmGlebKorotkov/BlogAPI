using System.ComponentModel.DataAnnotations;
using BlogAPI.Models.Enums;

namespace BlogAPI.Models;

public class UserProfileDto
{
    [Required]
    [MinLength(1)]
    public string Id { get; set; }

    [Required]
    public DateTime CreateTime { get; set; }

    [Required]
    [MinLength(1)]
    public string FullName { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required]
    public GenderEnum Gender { get; set; }

    [Required]
    [EmailAddress]
    [MinLength(1)]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
}