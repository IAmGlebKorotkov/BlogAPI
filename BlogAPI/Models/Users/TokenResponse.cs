using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models;

public class TokenResponse
{
    [Required]
    [MinLength(1)]
    public string Token { get; set; }
}