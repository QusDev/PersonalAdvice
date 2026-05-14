using Server.Data.Entities;

namespace Server.Repositories.Interfaces
{
    public interface IMovieRepository : IGenericRepository<MovieEntity>
    {
        Task<bool> IsExistMovieAsync (string title, int year);
        Task DeleteAllAsync();
    }
}
