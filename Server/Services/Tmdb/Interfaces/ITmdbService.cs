using Shared;

namespace Server.Services.Tmdb.Interfaces
{
    public interface ITmdbService
    {
        Task<Result<int>> ImportMovieWithCreditsAsync(int tmdbId);
        Task<Result<int>> ImportPopularMoviesAsync(int pageNumber, int pageCount);
    }
}
