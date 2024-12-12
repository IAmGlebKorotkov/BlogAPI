using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Post;

public class CreatePostDto
{
    [Required]
    [MinLength(5)]
    [MaxLength(1000)]
    public string Title { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(5000)]
    public string Description { get; set; }

    [Required]
    public int ReadingTime { get; set; }

    [Url]
    [MaxLength(1000)]
    public string Image { get; set; }

    public Guid? AddressId { get; set; }

    [Required]
    [MinLength(1)]
    public string Tags { get; set; }
}