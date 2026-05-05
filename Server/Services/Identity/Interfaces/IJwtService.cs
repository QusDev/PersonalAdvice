using Server.Data.Entities.Identity;

namespace Server.Services.Identity.Interfaces
{
    public interface IJwtService
    {
        public (string token, DateTime expires) GenerateJwtToken(ApplicationUser user, List<string> roles);
    }
}
