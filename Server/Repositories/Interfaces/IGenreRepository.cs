using Server.Data.Entities;

namespace Server.Repositories.Interfaces
{
    public interface IGenreRepository : IGenericRepository<GenreEntity>
    {
        Task<bool> IsExistByNameAsync(string name);
        Task<GenreEntity?> GetByNameAsync(string name);
    }
}
