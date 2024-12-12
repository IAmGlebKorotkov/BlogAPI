using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models.Post;

public class PostTag
{
    [Key]
    [Column("post_id")]
    public Guid PostId { get; set; }

    [Key]
    [Column("tag_id")]
    public Guid TagId { get; set; }

    [ForeignKey("PostId")]
    public PostDto Post { get; set; }

    [ForeignKey("TagId")]
    public TagDto Tag { get; set; }
}