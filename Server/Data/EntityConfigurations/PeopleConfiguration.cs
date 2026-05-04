using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Data.Entities;

namespace Server.Data.EntityConfigurations
{
    public class PeopleConfiguration : IEntityTypeConfiguration<PeopleEntity>
    {
        public void Configure(EntityTypeBuilder<PeopleEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
