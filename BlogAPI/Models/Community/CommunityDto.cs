using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Community;

public class CommunityDto
{
    [Required]
    public string Id { get; set; }

    [Required]
    public DateTime CreateTime { get; set; }

    [Required]
    [MinLength(1)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public bool IsClosed { get; set; } = false;

    [Required]
    public int SubscribersCount { get; set; } = 0;
}