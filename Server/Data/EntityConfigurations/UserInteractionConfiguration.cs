using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Data.Entities;

namespace Server.Data.EntityConfigurations
{
    public class UserInteractionConfiguration : IEntityTypeConfiguration<UserInteractionEntity>
    {
        public void Configure(EntityTypeBuilder<UserInteractionEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.UserId);

            builder
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.UserInteractions)
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.MediaContent)
                .WithMany(x => x.UserInteractions)
                .HasForeignKey(x => x.MediaId);
        }
    }
}
