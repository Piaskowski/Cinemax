
namespace Cinemax.Shared.Contracts.Identity
{
    public class ResetPasswordRequest
    {
        public Guid UserId { get; set; }
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}
