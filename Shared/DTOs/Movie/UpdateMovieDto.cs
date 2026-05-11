using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Movie
{
    public class UpdateMovieDto
    {
        [Required]
        public int Id { get; set; }
        public double? DurationMinutes { get; set; } = null;
        public bool? Adult { get; set; } = null;

        public string? Title { get; set; } = null;
        [MaxLength(2000)]
        public string? Description { get; set; } = null;
        public int? ReleaseYear { get; set; } = null;
        public string? PhotoUrl { get; set; } = null;
        [Range(0, 10)]
        public double? AverageRating { get; set; } = null;

        public List<int>? GenreIds { get; set; } = null;

    }
}
