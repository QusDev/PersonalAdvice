using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories.Interfaces;

namespace Server.Repositories
{
    public class TrackRepository : GenericRepository<TrackEntity>, ITrackRepository
    {
        public TrackRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
