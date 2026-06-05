namespace Server.Data.MLNet
{
    public class MediaRatingPrediction
    {
        public float Label { get; set; } 
        public uint UserId { get; set; }
        public uint MediaId { get; set; }
    }
}
