using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Data.Entities;

namespace Server.Data.EntityConfigurations
{
    public class MediaContentConfiguration : IEntityTypeConfiguration<MediaContentEntity>
    {
        public void Configure(EntityTypeBuilder<MediaContentEntity> builder)
        {
            builder.ToTable("MediaContents");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.AverageRating)
                .HasPrecision(3, 2); 
        }
    }
}
