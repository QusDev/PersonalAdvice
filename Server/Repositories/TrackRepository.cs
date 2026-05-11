using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> IsExistsTrackAsync(string title, string albumName)
        {
            return await _dbSet.AnyAsync(x => x.Title == title && x.AlbumName == albumName);
        }
    }
}
