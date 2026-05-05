namespace Shared.DTOs.Identity
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime Expires { get; set; }
    }
}
