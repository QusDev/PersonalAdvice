using Shared.DTOs.Entities;
using Shared.DTOs.Movie;
using Shared.DTOs.Repositories;

namespace Client.Services.Interfaces
{
    public interface IMovieService
    {
        Task<PagedResponse<MovieDto>?> GetMoviesAsync(GetAllMovieDto dto);
        Task<(bool isSuccess, string message)> CreateMovieAsync(CreateMovieDto dto);
        Task<(bool isSuccess, string message)> UpdateMovieAsync(UpdateMovieDto dto);
        Task<(bool isSuccess, string message)> DeleteMovieAsync(int id);
        Task<(bool isSuccess, string message)> ImportFromTmdbAsync(int pageNumber, int pageCount);
    }
}
