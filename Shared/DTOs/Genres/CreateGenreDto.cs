using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Genres
{
    public class CreateGenreDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
