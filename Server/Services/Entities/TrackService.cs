using AutoMapper;
using Server.Data.Entities;
using Server.Services.Entities.Interfaces;
using Server.UnitOfWork;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Repositories;
using Shared.DTOs.Tracks;

namespace Server.Services.Entities
{
    public class TrackService : ITrackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAsync(CreateTrackDto dto)
        {
            var track = _mapper.Map<TrackEntity>(dto);

            track.Type = Shared.Enums.MediaType.Music;

            if (dto.GenreIds.Any())
            {
                var genres = await _unitOfWork.Genres.GetAllAsync(filter: g => dto.GenreIds.Contains(g.Id));
                track.Genres = genres.Items.ToList();
            }

            await _unitOfWork.Tracks.AddAsync(track);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var track = await _unitOfWork.Tracks.GetByIdAsync(id);

            if (track == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Track with id: {id} not found"));
            }

            _unitOfWork.Tracks.Delete(track);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<PagedResponse<TrackDto>>> GetAllAsync(GetAllTrackDto dto)
        {
            var tracks = await _unitOfWork.Tracks.GetAllAsync(
                pageNumber: dto.PageNumber,
                pageSize: dto.PageSize,
                includes:
                [
                    m => m.Genres,
                    m => m.MediaCollaborators,
                ]);

            var result = _mapper.Map<PagedResponse<TrackDto>>(tracks);
            return Result<PagedResponse<TrackDto>>.Success(result);
        }

        public async Task<Result<TrackDto>> GetByIdAsync(int id)
        {
            var track = await _unitOfWork.Tracks.GetByIdAsync(
                id,
                includes:
                [
                    m => m.Genres,
                    m => m.MediaCollaborators,
                ]);

            if (track == null)
            {
                return Result<TrackDto>.Fail(Error.NotFound($"Track with id: {id} not found"));
            }

            var trackDto = _mapper.Map<TrackDto>(track);
            return Result<TrackDto>.Success(trackDto);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateTrackDto dto)
        {
            var track = await _unitOfWork.Tracks.GetByIdAsync(dto.Id, includes: m => m.Genres);

            if (track == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Track with id: {dto.Id} not found"));
            }

            _mapper.Map(dto, track);

            if (dto.GenreIds != null && dto.GenreIds.Any())
            {
                var selectedGenres = await _unitOfWork.Genres.GetAllAsync(filter: g => dto.GenreIds.Contains(g.Id));
                track.Genres.Clear();

                foreach (var genre in selectedGenres.Items)
                {
                    track.Genres.Add(genre);
                }
            }

            _unitOfWork.Tracks.Update(track);
            await _unitOfWork.SaveAsync();
            return Result<bool>.Success(true);
        }
    }
}
