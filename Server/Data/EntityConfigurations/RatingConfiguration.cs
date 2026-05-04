using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Data.Entities;

namespace Server.Data.EntityConfigurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<RatingEntity>
    {
        public void Configure(EntityTypeBuilder<RatingEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.MediaContent)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.MediaId);
        }
    }
}
