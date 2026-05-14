using Shared.DTOs.MediaContent;
using Shared.DTOs.Repositories;

namespace Client.Services.Interfaces
{
    public interface IMediaService
    {
        Task<PagedResponse<MediaCardDto>> GetUserMediaRecommendationsAsync(GetUserMediaRecommendationsDto dto);
    }
}
