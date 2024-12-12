using System.ComponentModel.DataAnnotations;
using BlogAPI.Models.Enums;

namespace BlogAPI.Models.Author;

public class AuthorDto
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MinLength(1)]
    public string FullName { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required]
    public GenderEnum Gender { get; set; }

    public int Posts { get; set; }

    public int Likes { get; set; }

    public DateTime Created { get; set; }
}