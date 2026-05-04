using Server.Data.Constants;

namespace Server.Data.Entities
{
    public class GenreEntity : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<MediaContentEntity> MediaContents { get; set; } = null!;
    }
}
