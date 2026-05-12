using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.MediaContent
{
    public class CatalogMediaDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PageSize { get; set; } = 10;

        public string? SearchTitle { get; set; }
        public MediaType? Type { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public double? RatingFrom { get; set; }
        public double? RatingTo { get; set; }

    }
}
