using Server.Data.Entities;
using Shared.DTOs.MediaContent;
using Shared.DTOs.Repositories;

namespace Server.Repositories.Interfaces
{
    public interface IMediaRepository : IGenericRepository<MediaContentEntity>
    {
        Task<PagedResponse<MediaContentEntity>> GetFiltered(CatalogMediaDto dto);
    }
}
