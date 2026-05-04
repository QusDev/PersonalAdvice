using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Data.EntityConfigurations
{
    public class TrackConfiguration : IEntityTypeConfiguration<TrackConfiguration>
    {
        public void Configure(EntityTypeBuilder<TrackConfiguration> builder)
        {
        }
    }
}
