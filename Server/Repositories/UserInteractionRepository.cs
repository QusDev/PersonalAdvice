using Microsoft.EntityFrameworkCore;
using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories.Interfaces;

namespace Server.Repositories
{
    public class UserInteractionRepository : GenericRepository<UserInteractionEntity>, IUserInteractionRepository
    {
        public UserInteractionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CreateOrUpdate(UserInteractionEntity userInteractionEntity)
        {
            var userInteractionExist = _dbSet.FirstOrDefault(ui => ui.UserId == userInteractionEntity.UserId && ui.MediaId == userInteractionEntity.MediaId);
            if (userInteractionExist == null)
            {
                _dbSet.Add(userInteractionEntity);
            }
            else
            {
                userInteractionExist.Weight = userInteractionEntity.Weight;
                userInteractionExist.Type = userInteractionEntity.Type;
                _dbSet.Update(userInteractionExist);
            }
        }

        public async Task<UserInteractionEntity?> GetByMediaAndUserAsync(int mediaId, int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(ui => ui.MediaId == mediaId && ui.UserId == userId);
        }
    }
}
