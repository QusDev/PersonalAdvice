using Shared;
using Shared.DTOs.MediaContent;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities.Interfaces
{
    public interface IMediaService
    {
        Task<Result<PagedResponse<SearchMediaItemDto>>> SearchMediaAsync(SearchMediaDto dto);
    }
}
