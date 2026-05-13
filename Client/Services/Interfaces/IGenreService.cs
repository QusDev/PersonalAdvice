using Shared.DTOs.Entities;
using Shared.DTOs.Genres;
using Shared.DTOs.Repositories;

namespace Client.Services.Interfaces
{
    public interface IGenreService
    {
        Task<PagedResponse<GenreDto>?> GetGenresAsync(GetAllGenreDto dto);
        Task<(bool isSuccess, string message)> CreateGenreAsync(CreateGenreDto dto);
        Task<(bool isSuccess, string message)> UpdateGenreAsync(UpdateGenreDto dto);
        Task<(bool isSuccess, string message)> DeleteGenreAsync(int id);
    }
}
