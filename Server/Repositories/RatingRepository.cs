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
    }
}
