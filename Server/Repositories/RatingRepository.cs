using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories.Interfaces;

namespace Server.Repositories
{
    public class RatingRepository : GenericRepository<RatingEntity>, IRatingRepository
    {
        public RatingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> AddRangeAsync(List<RatingEntity> ratings)
        {
            await _dbSet.AddRangeAsync(ratings);
            return true;
        }

        public bool RemoveRange(List<RatingEntity> ratings)
        {
            _dbSet.RemoveRange(ratings);
            return true;
        }
    }
}
