using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Tracks
{
    public class UpdateTrackDto
    {
        [Required]
        public int Id { get; set; }
        public string AlbumName { get; set; } = null!;
        public string? Lyrics { get; set; }
        public string? AudioUrl { get; set; }

        public string? Title { get; set; } = null;
        [MaxLength(2000)]
        public string? Description { get; set; } = null;
        public int? ReleaseYear { get; set; } = null;
        public string? PhotoUrl { get; set; } = null;
        [Range(0, 10)]
        public double? AverageRating { get; set; } = null;
        public double DurationMinutes { get; set; }

        public List<int>? GenreIds { get; set; } = null;
    }
}
