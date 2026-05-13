using System.Text.Json.Serialization;

namespace Shared.DTOs.Jamendo
{
    public class JamendoStats
    {
        [JsonPropertyName("avgnote")]
        public double? Avgnote { get; set; }
    }
}
