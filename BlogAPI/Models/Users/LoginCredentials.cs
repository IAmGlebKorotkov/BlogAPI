using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models;

public class LoginCredentials
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}