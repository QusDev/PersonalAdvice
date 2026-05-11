using System.Text.Json.Serialization;

namespace Shared.DTOs.TMDBs
{
    public class TmdbPopularItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = null!;
    }
}
