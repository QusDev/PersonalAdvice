using System.Text.Json.Serialization;

namespace Shared.DTOs.Jamendo
{
    public class JamendoTrack
    {
        public string Id { get; set; } = null!;
        
        public string Name { get; set; } = null!;
        
        [JsonPropertyName("album_name")]
        public string AlbumName { get; set; } = null!;
        
        [JsonPropertyName("artist_id")]
        public string ArtistId { get; set; } = null!;
        
        [JsonPropertyName("artist_name")]
        public string ArtistName { get; set; } = null!;
        
        [JsonPropertyName("releasedate")]
        public string ReleaseDate { get; set; } = null!;
        
        public double Duration { get; set; }
        
        public string Image { get; set; } = null!;
        
        public string Audio { get; set; } = null!;
        
        public string? Lyrics { get; set; }
        public double PopularityMonth { get; set; }
        
        [JsonPropertyName("stats")]
        public JamendoStats Stats { get; set; } = null!;
        
        [JsonPropertyName("musicinfo")]
        public JamendoMusicInfo MusicInfo { get; set; } = new();
    }
}
