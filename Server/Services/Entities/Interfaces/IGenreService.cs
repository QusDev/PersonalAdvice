using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Genres;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities.Interfaces
{
    public interface IGenreService
    {
        Task<Result<bool>> AddAsync(CreateGenreDto dto);
        Task<Result<bool>> UpdateAsync(UpdateGenreDto dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<PagedResponse<GenreDto>>> GetAllAsync(GetAllGenreDto dto);
        Task<Result<GenreDto>> GetByIdAsync(int id);
    }
}
