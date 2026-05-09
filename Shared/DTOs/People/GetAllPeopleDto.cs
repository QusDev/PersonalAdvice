using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.People
{
    public class GetAllPeopleDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PageSize { get; set; } = 10;
    }
}
