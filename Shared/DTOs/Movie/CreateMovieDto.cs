using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Movie
{
    public class CreateMovieDto
    {
        [Required]
        public int DurationMinutes { get; set; }
        [Required]
        public bool Adult { get; set; }

        [Required]
        public string Title { get; set; } = null!;
        [MaxLength(2000)]
        public string? Description { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        public string? PhotoUrl { get; set; }
        [Range(0, 10)]
        public double AverageRating { get; set; }

        [Required(ErrorMessage = "Choose at least one genre")]
        public List<int> GenreIds { get; set; } = new();
    }
}
