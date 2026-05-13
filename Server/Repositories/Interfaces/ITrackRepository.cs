using Server.Data.Entities;

namespace Server.Repositories.Interfaces
{
    public interface ITrackRepository : IGenericRepository<TrackEntity>
    {
        Task<bool> IsExistsTrackAsync(string title, string albumName);
        Task DeleteAllAsync();
    }
}
