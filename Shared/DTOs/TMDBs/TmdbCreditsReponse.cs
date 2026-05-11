namespace Shared.DTOs.TMDBs
{
    public class TmdbCreditsReponse
    {
        public List<TmdbCast> Cast { get; set; } = new();
        public List<TmdbCrew> Crew { get; set; } = new();

    }
}
