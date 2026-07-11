using Cinemax.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>, IAuditable
    {
        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public required string LastName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        [MaxLength(320)]
        public string? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
