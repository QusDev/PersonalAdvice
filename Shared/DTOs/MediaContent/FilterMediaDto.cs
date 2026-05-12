using Shared.Enums;

namespace Shared.DTOs.MediaContent
{
    public class FilterMediaDto
    {
        public string? SearchTitle { get; set; }
        public MediaType? Type { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public double? RatingFrom { get; set; }
        public double? RatingTo { get; set; }
    }
}
