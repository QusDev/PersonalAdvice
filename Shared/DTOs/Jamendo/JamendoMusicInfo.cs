using System.Text.Json.Serialization;

namespace Shared.DTOs.Jamendo
{
    public class JamendoMusicInfo
    {
        [JsonPropertyName("tags")]
        public JamendoTags Tags { get; set; } = new();
    }
}
