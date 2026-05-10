using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories;
using Server.Repositories.Interfaces;

namespace Server.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IMovieRepository? _movies;
        private IGenreRepository? _genres;
        private ITrackRepository? _tracks;
        private IPeopleRepository? _people;
        private IRatingRepository? _ratings;
        private IUserInteractionRepository? _userInteractions;
        private IMediaCollaboratorRepository? _mediaCollaborators;
        private IGenericRepository<MediaContentEntity>? _mediaContent;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IMovieRepository Movies => _movies ??= new MovieRepository(_context);

        public IGenreRepository Genres => _genres ??= new GenreRepository(_context);

        public ITrackRepository Tracks => _tracks ??= new TrackRepository(_context);

        public IPeopleRepository People => _people ??= new PeopleRepository(_context);

        public IRatingRepository Ratings => _ratings ??= new RatingRepository(_context);

        public IUserInteractionRepository UserInteractions => _userInteractions ??= new UserInteractionRepository(_context);

        public IMediaCollaboratorRepository MediaCollaborators => _mediaCollaborators ??= new MediaCollaboratorRepository(_context);

        public IGenericRepository<MediaContentEntity> MediaContent => _mediaContent ??= new GenericRepository<MediaContentEntity>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}
