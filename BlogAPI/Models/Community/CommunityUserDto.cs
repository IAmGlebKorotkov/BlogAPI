using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Community;

public class CommunityUserDto
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string CommunityId { get; set; }

    [Required]
    public CommunityRole Role { get; set; }
}