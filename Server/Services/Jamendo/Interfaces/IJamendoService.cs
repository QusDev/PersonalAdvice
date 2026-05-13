using Shared;

namespace Server.Services.Jamendo.Interfaces
{
    public interface IJamendoService
    {
        Task<Result<int>> ImportTrendingTracksAsync(int page, int pageCount);
    }
}
