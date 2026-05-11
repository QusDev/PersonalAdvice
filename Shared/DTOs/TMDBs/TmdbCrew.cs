using System.Text.Json.Serialization;

namespace Shared.DTOs.TMDBs
{
    public class TmdbCrew
    {
        public string Name { get; set; } = null!;
        public string Job { get; set; } = null!;
        [JsonPropertyName("profile_path")]
        public string? ProfilePath { get; set; }
    }
}
