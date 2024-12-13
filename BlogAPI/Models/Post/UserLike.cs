using System.ComponentModel.DataAnnotations.Schema;
using BlogAPI.Models.Author;

namespace BlogAPI.Models.Post;

public class UserLike
{
    [Column("id")]
    public int Id { get; set; }
    [Column("user_id")]
    public string UserId { get; set; } 
    [Column("post_id")]
    public Guid PostId { get; set; } 
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Навигационные свойства (опционально)
    public UserDto User { get; set; }
    public PostDto Post { get; set; }
}