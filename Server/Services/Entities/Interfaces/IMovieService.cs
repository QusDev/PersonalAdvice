using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Movie;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities.Interfaces
{
    public interface IMovieService
    {
        Task<Result<int>> AddAsync(CreateMovieDto dto);
        Task<Result<bool>> UpdateAsync(UpdateMovieDto dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<PagedResponse<MovieDto>>> GetAllAsync(GetAllMovieDto dto);
        Task<Result<MovieDto>> GetByIdAsync(int id);
        Task<bool> IsExistsMovieAsync(string title, int year);
        Task DeleteAllAsync();
    }
}
