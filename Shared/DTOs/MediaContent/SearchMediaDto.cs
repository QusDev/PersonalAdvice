using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.MediaContent
{
    public class SearchMediaDto
    {
        [Required]
        public string Title { get; set; } = null!;
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PageSize { get; set; } = 10;
    }
}
