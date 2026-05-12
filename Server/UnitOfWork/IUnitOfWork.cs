using Server.Data.Entities;
using Server.Repositories.Interfaces;
using Server.Services.Entities.Interfaces;

namespace Server.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movies { get; }
        IGenreRepository Genres { get; }
        ITrackRepository Tracks { get; }
        IPeopleRepository People { get; }
        IRatingRepository Ratings { get; }
        IUserInteractionRepository UserInteractions { get; }
        IMediaCollaboratorRepository MediaCollaborators { get; }
        IMediaRepository MediaContent { get; }

        Task<int> SaveAsync();
    }
}
