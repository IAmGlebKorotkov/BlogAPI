using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models;

public class TagDto
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }

    [Column("CreateTime")]
    public DateTime CreateTime { get; set; }

    [Column("Name")]
    public string Name { get; set; }
}