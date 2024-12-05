using System.Text.Json.Serialization;

namespace BlogAPI.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GenderEnum
{
    Male,
    Female
}