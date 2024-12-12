using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models;

public class TagDto
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }

    [Column("createtime")]
    public DateTime CreateTime { get; set; }

    [Column("name")] 
    public string Name { get; set; }
}