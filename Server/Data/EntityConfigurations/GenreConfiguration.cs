using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Data.Entities;

namespace Server.Data.EntityConfigurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<GenreEntity>
    {
        public void Configure(EntityTypeBuilder<GenreEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasMany(g => g.MediaContents)
                .WithMany(b => b.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "MediaGenres",
                    j => j
                        .HasOne<MediaContentEntity>()
                        .WithMany()
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<GenreEntity>()
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("MediaId", "GenreId");
                        j.HasIndex("MediaId", "GenreId").IsUnique();
                    }
                );
        }
    }
}
