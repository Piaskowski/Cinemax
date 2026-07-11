

using Cinemax.Shared.Contracts.Common;

namespace Cinemax.Shared.Contracts.Auth
{
    public class LoginResponse : GeneralResponse
    {
        public string? Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
