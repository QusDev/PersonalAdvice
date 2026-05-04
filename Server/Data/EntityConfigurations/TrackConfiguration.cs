using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Data.Entities;

namespace Server.Data.EntityConfigurations
{
    public class TrackConfiguration : IEntityTypeConfiguration<TrackEntity>
    {
        public void Configure(EntityTypeBuilder<TrackEntity> builder)
        {
            builder.ToTable("Tracks");
        }
    }
}
