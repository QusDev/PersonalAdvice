using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories;
using Server.Repositories.Interfaces;

namespace Server.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<MovieEntity>? _movies;
        private IGenreRepository? _genres;
        private IGenericRepository<TrackEntity>? _tracks;
        private IGenericRepository<PeopleEntity>? _people;
        private IGenericRepository<RatingEntity>? _ratings;
        private IGenericRepository<UserInteractionEntity>? _userInteractions;
        private IGenericRepository<MediaCollaboratorEntity>? _mediaCollaborators;
        private IGenericRepository<MediaContentEntity>? _mediaContent;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<MovieEntity> Movies => _movies ??= new GenericRepository<MovieEntity>(_context);

        public IGenreRepository Genres => _genres ??= new GenreRepository(_context);

        public IGenericRepository<TrackEntity> Tracks => _tracks ??= new GenericRepository<TrackEntity>(_context);

        public IGenericRepository<PeopleEntity> People => _people ??= new GenericRepository<PeopleEntity>(_context);

        public IGenericRepository<RatingEntity> Ratings => _ratings ??= new GenericRepository<RatingEntity>(_context);

        public IGenericRepository<UserInteractionEntity> UserInteractions => _userInteractions ??= new GenericRepository<UserInteractionEntity>(_context);

        public IGenericRepository<MediaCollaboratorEntity> MediaCollaborators => _mediaCollaborators ??= new GenericRepository<MediaCollaboratorEntity>(_context);

        public IGenericRepository<MediaContentEntity> MediaContent => _mediaContent ??= new GenericRepository<MediaContentEntity>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}
