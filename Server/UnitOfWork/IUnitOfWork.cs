using Server.Data.Entities;
using Server.Repositories.Interfaces;
using Server.Services.Entities.Interfaces;

namespace Server.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<MovieEntity> Movies { get; }
        IGenreRepository Genres { get; }
        IGenericRepository<TrackEntity> Tracks { get; }
        IGenericRepository<PeopleEntity> People { get; }
        IGenericRepository<RatingEntity> Ratings { get; }
        IGenericRepository<UserInteractionEntity> UserInteractions { get; }
        IGenericRepository<MediaCollaboratorEntity> MediaCollaborators { get; }
        IGenericRepository<MediaContentEntity> MediaContent { get; }

        Task<int> SaveAsync();
    }
}
