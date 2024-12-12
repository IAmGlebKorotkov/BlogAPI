using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Community;
public class CommunityDto
{
    public string Id { get; set; } 
    public DateTime CreateTime { get; set; }
    [MinLength(1)]
    public string Name { get; set; } 
    public string? Description { get; set; }
    public bool IsClosed { get; set; } = false; 
    public int SubscribersCount { get; set; } = 0; 
}
