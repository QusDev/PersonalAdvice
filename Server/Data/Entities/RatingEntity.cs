using Server.Data.Constants;

namespace Server.Data.Entities
{
    public class RatingEntity : CreatedEntity
    {
        public int UserId { get; set; }
        public int MediaId { get; set; }
        public double Score { get; set; }
    }
}
