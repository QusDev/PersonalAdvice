using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Tracks
{
    public class CreateTrackDto
    {
        [Required]
        public string AlbumName { get; set; } = null!;
        public string? Lyrics { get; set; }
        public string? AudioUrl { get; set; }

        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public int ReleaseYear { get; set; }
        public string? PhotoUrl { get; set; }
        [Range(0, 10)]
        public double AverageRating { get; set; }
        [Required]
        public double DurationMinutes { get; set; }

        [Required(ErrorMessage = "Choose at least one genre")]
        public List<int> GenreIds { get; set; } = new();
    }
}
