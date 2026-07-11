
namespace Cinemax.Shared.Settings
{
    public class AuthSettings
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int TokenExpirationDays { get; set; }
    }
}
