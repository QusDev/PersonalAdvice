using Server.Data.Entities;

namespace Server.Repositories.Interfaces
{
    public interface IMovieRepository : IGenericRepository<MovieEntity>
    {
        Task<bool> IsExistByTitleAsync(string title);
    }
}
