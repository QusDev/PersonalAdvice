using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Data.Entities;

namespace Server.Data.EntityConfigurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<MovieEntity>
    {
        public void Configure(EntityTypeBuilder<MovieEntity> builder)
        {
        }
    }
}
