using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Data.Entities;
using Server.Data.Entities.Identity;

namespace Server.Data.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<MediaContentEntity> MediaContents { get; set; } = null!;
        public DbSet<MovieEntity> Movies { get; set; } = null!;
        public DbSet<TrackEntity> Tracks { get; set; } = null!;
        public DbSet<GenreEntity> Genres { get; set; } = null!;
        public DbSet<PeopleEntity> People { get; set; } = null!;
        public DbSet<MediaCollaboratorEntity> MediaCollaborators { get; set; } = null!;
        public DbSet<RatingEntity> Ratings { get; set; } = null!;
        public DbSet<UserInteractionEntity> UserInteractions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
