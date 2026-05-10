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
    }
}
