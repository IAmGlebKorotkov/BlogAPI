using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogAPI.Models.Post;

namespace BlogAPI.Models.Author;

public class PostDto
{
    [Key]
    [Column("id")] 
    public Guid Id { get; set; }

    [Required]
    [Column("create_time")]
    public DateTime CreateTime { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; }

    [Required]
    [Column("description")]
    public string Description { get; set; }

    [Required]
    [Column("reading_time")]
    public int ReadingTime { get; set; }

    [Column("image")]
    public string Image { get; set; }

    [Column("address_id")]
    public Guid? AddressId { get; set; }

    [Required]
    [Column("author_id")]
    public Guid AuthorId { get; set; }

    [Required]
    [Column("author")]
    public string Author { get; set; }

    [Column("community_id")]
    public string? CommunityId { get; set; }

    [Column("community_name")]
    public string? CommunityName { get; set; }

    [Required]
    [Column("tag_posts")]
    public string TagPosts { get; set; }

    [Required]
    [Column("likes")]
    public int Likes { get; set; } = 0;

    [Required]
    [Column("has_like")]
    public bool HasLike { get; set; } = false;

    [Required]
    [Column("comments_count")]
    public int CommentsCount { get; set; } = 0;
}