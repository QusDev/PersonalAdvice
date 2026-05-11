using System.Text.Json.Serialization;

namespace Shared.DTOs.TMDBs
{
    public class TmdbPopularResponse
    {
        public int Page { get; set; }
        public List<TmdbPopularItem> Results { get; set; } = new();
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }
}
