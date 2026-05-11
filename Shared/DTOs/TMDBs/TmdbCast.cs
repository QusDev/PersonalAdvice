using System.Text.Json.Serialization;

namespace Shared.DTOs.TMDBs
{
    public class TmdbCast
    {
        public string Name { get; set; } = null!;
        public string? Character { get; set; }
        [JsonPropertyName("profile_path")]
        public string? ProfilePath { get; set; }
    }
}
