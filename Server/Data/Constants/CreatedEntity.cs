namespace Server.Data.Constants
{
    public class CreatedEntity : BaseEntity, ICreated
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
