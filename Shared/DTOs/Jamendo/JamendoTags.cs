using System.Text.Json.Serialization;

namespace Shared.DTOs.Jamendo
{
    public class JamendoTags
    {
        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; } = new();
    }
}
