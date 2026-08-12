using Cinemax.Shared.Enums;
using Cinemax.Shared.Resources.Validation;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.Identity
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
