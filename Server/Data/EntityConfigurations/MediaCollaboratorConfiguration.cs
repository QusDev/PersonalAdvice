using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Data.Entities;

namespace Server.Data.EntityConfigurations
{
    public class MediaCollaboratorConfiguration : IEntityTypeConfiguration<MediaCollaboratorEntity>
    {
        public void Configure(EntityTypeBuilder<MediaCollaboratorEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.MediaContent)
                .WithMany(x => x.MediaCollaborators)
                .HasForeignKey(x => x.MediaId);

            builder
                .HasOne(x => x.People)
                .WithMany(x => x.MediaCollaborators)
                .HasForeignKey(x => x.PersonId);
        }
    }
}
