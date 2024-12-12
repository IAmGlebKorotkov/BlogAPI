using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Post;

public class PostDto
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime CreateTime { get; set; }

    [Required]
    [MinLength(1)]
    public string Title { get; set; }

    [Required]
    [MinLength(1)]
    public string Description { get; set; }

    [Required]
    public int ReadingTime { get; set; }

    public string Image { get; set; }

    [Required]
    public Guid AuthorId { get; set; }

    [Required]
    [MinLength(1)]
    public string Author { get; set; }

    public Guid? CommunityId { get; set; }

    public string CommunityName { get; set; }

    public Guid? AddressId { get; set; }

    public int Likes { get; set; } = 0;

    public bool HasLike { get; set; } = false;

    public int CommentsCount { get; set; } = 0;

    public List<TagDto> Tags { get; set; }
}