using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.People;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities.Interfaces
{
    public interface IPeopleService
    {
        Task<Result<bool>> AddAsync(CreatePeopleDto dto);
        Task<Result<bool>> UpdateAsync(UpdatePeopleDto dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<PagedResponse<PeopleDto>>> GetAllAsync(GetAllPeopleDto dto);
        Task<Result<PeopleDto>> GetByIdAsync(int id);
    }
}
