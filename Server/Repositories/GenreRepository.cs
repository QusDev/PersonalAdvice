using Microsoft.EntityFrameworkCore;
using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories.Interfaces;

namespace Server.Repositories
{
    public class GenreRepository : GenericRepository<GenreEntity>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> IsExistByNameAsync(string name)
        {
            return await _dbSet.AnyAsync(x => x.Name == name);
        }
    }
}
