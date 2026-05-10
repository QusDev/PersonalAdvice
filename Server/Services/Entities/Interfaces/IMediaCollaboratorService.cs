using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.MediaCollaborator;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities.Interfaces
{
    public interface IMediaCollaboratorService
    {
        Task<Result<bool>> AddAsync(CreateMediaCollaboratorDto dto);
        Task<Result<bool>> UpdateAsync(UpdateMediaCollaboratorDto dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<PagedResponse<MediaCollaboratorDto>>> GetAllAsync(GetAllMediaCollaboratorDto dto);
        Task<Result<MediaCollaboratorDto>> GetByIdAsync(int id);
    }
}
