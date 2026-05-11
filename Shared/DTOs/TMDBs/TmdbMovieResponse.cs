using System.Text.Json.Serialization;

namespace Shared.DTOs.TMDBs
{
    public class TmdbMovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Overview { get; set; }
        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = null!;
        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }
        public int Runtime { get; set; }
        public bool Adult { get; set; }
        public List<TmdbGenre> Genres { get; set; } = new();
    }
}
