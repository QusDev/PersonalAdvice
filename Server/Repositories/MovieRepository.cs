using Microsoft.EntityFrameworkCore;
using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories.Interfaces;

namespace Server.Repositories
{
    public class MovieRepository : GenericRepository<MovieEntity>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> IsExistByTitleAsync(string title)
        {
            return await _dbSet.AnyAsync(x => x.Title == title);
        }
    }
}
