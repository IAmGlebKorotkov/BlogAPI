using System.Text.Json.Serialization;

namespace BlogAPI.Models.Post;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PostSorting
{
    CreateDesc,
    CreateAsc,
    LikeAsc,
    LikeDesc
}