using Shared;
using Shared.DTOs.MediaContent;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities.Interfaces
{
    public interface IMediaService
    {
        Task<Result<PagedResponse<MediaCardDto>>> SearchMediaAsync(SearchMediaDto dto);
        Task<Result<PagedResponse<MediaCardDto>>> CatalogMediaAsync(CatalogMediaDto dto);
        Task<Result<PagedResponse<MediaCardDto>>> GetUserMediaRecommendationsAsync(GetUserMediaRecommendationsDto dto);
    }
}
