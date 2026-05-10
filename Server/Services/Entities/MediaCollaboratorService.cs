using AutoMapper;
using Server.Data.Entities;
using Server.Services.Entities.Interfaces;
using Server.UnitOfWork;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.MediaCollaborator;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities
{
    public class MediaCollaboratorService : IMediaCollaboratorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MediaCollaboratorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAsync(CreateMediaCollaboratorDto dto)
        {
            var mediaCollaborator = _mapper.Map<MediaCollaboratorEntity>(dto);

            await _unitOfWork.MediaCollaborators.AddAsync(mediaCollaborator);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var mediaCollaborator = await _unitOfWork.MediaCollaborators.GetByIdAsync(id);

            if (mediaCollaborator == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Media collaborator with id: {id} not found"));
            }

            _unitOfWork.MediaCollaborators.Delete(mediaCollaborator);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<PagedResponse<MediaCollaboratorDto>>> GetAllAsync(GetAllMediaCollaboratorDto dto)
        {
            var mediaCollaborators = await _unitOfWork.MediaCollaborators.GetAllAsync(pageNumber: dto.PageNumber, pageSize: dto.PageSize);
            var result = _mapper.Map<PagedResponse<MediaCollaboratorDto>>(mediaCollaborators);
            return Result<PagedResponse<MediaCollaboratorDto>>.Success(result);
        }

        public async Task<Result<MediaCollaboratorDto>> GetByIdAsync(int id)
        {
            var mediaCollaborator = await _unitOfWork.MediaCollaborators.GetByIdAsync(id);

            if (mediaCollaborator == null)
            {
                return Result<MediaCollaboratorDto>.Fail(Error.NotFound($"Media collaborator with id: {id} not found"));
            }

            var genreDto = _mapper.Map<MediaCollaboratorDto>(mediaCollaborator);
            return Result<MediaCollaboratorDto>.Success(genreDto);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateMediaCollaboratorDto dto)
        {
            var mediaCollaborator = await _unitOfWork.MediaCollaborators.GetByIdAsync(dto.Id);

            if (mediaCollaborator == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Media collaborator with id: {dto.Id} not found"));
            }

            _mapper.Map(dto, mediaCollaborator);
            _unitOfWork.MediaCollaborators.Update(mediaCollaborator);
            await _unitOfWork.SaveAsync();
            return Result<bool>.Success(true);
        }
    }
}
