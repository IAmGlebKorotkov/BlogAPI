using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogAPI.Models.Community;

public class CommunityUserDto
{
    [Required]
    public string UserId { get; set; } 

    [Required]
    public string CommunityId { get; set; }

    [Required]
    public string Role { get; set; }
    
    [JsonIgnore] 
    public UserDto User { get; set; }

    [JsonIgnore]
    public CommunityDto Community { get; set; }
}