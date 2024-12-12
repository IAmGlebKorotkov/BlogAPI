using System.ComponentModel.DataAnnotations;
namespace BlogAPI.Models.Community;

public class CommunityFullDto : CommunityDto
{
    [Required]
    public List<UserProfileDto> Administrators { get; set; }
}
