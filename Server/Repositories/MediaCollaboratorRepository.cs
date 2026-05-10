using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories.Interfaces;

namespace Server.Repositories
{
    public class MediaCollaboratorRepository : GenericRepository<MediaCollaboratorEntity>, IMediaCollaboratorRepository
    {
        public MediaCollaboratorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
