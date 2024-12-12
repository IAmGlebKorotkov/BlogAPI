using System.Text.Json.Serialization;

namespace BlogAPI.Models.Community;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommunityRole
{
    Administrator,
    Subscriber
}