
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Domain.Entities.Notifications
{
    public class MessageTemplate
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Code; 
        [Required]
        [MaxLength(256)]
        public required string Subject { get; set; }
        [Required]
        [MaxLength(4000)]
        public required string Body { get; set; }
    }
}
