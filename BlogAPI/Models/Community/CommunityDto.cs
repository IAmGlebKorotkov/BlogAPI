using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.Community;
public class CommunityDto
{
    public string Id { get; set; } // Уникальный идентификатор сообщества
    public DateTime CreateTime { get; set; } // Дата создания сообщества
    [MinLength(1)]
    public string Name { get; set; } // Название сообщества
    public string? Description { get; set; } // Описание сообщества (может быть null)
    public bool IsClosed { get; set; } = false; // Флаг, указывающий, закрыто ли сообщество
    public int SubscribersCount { get; set; } = 0; // Количество подписчиков
}
