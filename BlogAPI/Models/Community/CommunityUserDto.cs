using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogAPI.Models.Community;

public class CommunityUserDto
{
    [Required]
    public string UserId { get; set; } // Первичный ключ

    [Required]
    public string CommunityId { get; set; } // Первичный ключ

    [Required]
    public string Role { get; set; }
    
    [JsonIgnore] // Исключаем поле из сериализации
    public UserDto User { get; set; }

    [JsonIgnore] // Исключаем поле из сериализации
    public CommunityDto Community { get; set; }
}