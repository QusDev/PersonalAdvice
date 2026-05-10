using AutoMapper;
using Server.Data.Entities;
using Server.Services.Entities.Interfaces;
using Server.UnitOfWork;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Repositories;
using Shared.DTOs.UserInteractions;

namespace Server.Services.Entities
{
    public class UserInteractionService : IUserInteractionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserInteractionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAsync(CreateUserInteractionDto dto)
        {
            var userInteraction = _mapper.Map<UserInteractionEntity>(dto);

            await _unitOfWork.UserInteractions.AddAsync(userInteraction);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var userInteraction = await _unitOfWork.UserInteractions.GetByIdAsync(id);

            if (userInteraction == null)
            {
                return Result<bool>.Fail(Error.NotFound($"User interaction with id: {id} not found"));
            }

            _unitOfWork.UserInteractions.Delete(userInteraction);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<PagedResponse<UserInteractionDto>>> GetAllAsync(GetAllUserInteractionDto dto)
        {
            var userInteractions = await _unitOfWork.UserInteractions.GetAllAsync(pageNumber: dto.PageNumber, pageSize: dto.PageSize);
            var result = _mapper.Map<PagedResponse<UserInteractionDto>>(userInteractions);
            return Result<PagedResponse<UserInteractionDto>>.Success(result);
        }

        public async Task<Result<UserInteractionDto>> GetByIdAsync(int id)
        {
            var userInteraction = await _unitOfWork.UserInteractions.GetByIdAsync(id);

            if (userInteraction == null)
            {
                return Result<UserInteractionDto>.Fail(Error.NotFound($"User interaction with id: {id} not found"));
            }

            var genreDto = _mapper.Map<UserInteractionDto>(userInteraction);
            return Result<UserInteractionDto>.Success(genreDto);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateUserInteractionDto dto)
        {
            var userInteraction = await _unitOfWork.UserInteractions.GetByIdAsync(dto.Id);

            if (userInteraction == null)
            {
                return Result<bool>.Fail(Error.NotFound($"User intercation with id: {dto.Id} not found"));
            }

            _mapper.Map(dto, userInteraction);
            _unitOfWork.UserInteractions.Update(userInteraction);
            await _unitOfWork.SaveAsync();
            return Result<bool>.Success(true);
        }
    }
}
