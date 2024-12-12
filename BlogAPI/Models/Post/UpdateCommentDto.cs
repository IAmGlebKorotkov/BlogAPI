using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Post;

public class UpdateCommentDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public string Content { get; set; }
}