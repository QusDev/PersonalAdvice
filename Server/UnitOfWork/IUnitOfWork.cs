using Server.Data.Entities;
using Server.Repositories.Interfaces;
using Server.Services.Entities.Interfaces;

namespace Server.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movies { get; }
        IGenreRepository Genres { get; }
        IGenericRepository<TrackEntity> Tracks { get; }
        IPeopleRepository People { get; }
        IGenericRepository<RatingEntity> Ratings { get; }
        IGenericRepository<UserInteractionEntity> UserInteractions { get; }
        IMediaCollaboratorRepository MediaCollaborators { get; }
        IGenericRepository<MediaContentEntity> MediaContent { get; }

        Task<int> SaveAsync();
    }
}
