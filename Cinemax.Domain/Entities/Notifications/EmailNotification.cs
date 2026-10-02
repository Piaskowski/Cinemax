using Cinemax.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Domain.Entities.Notifications
{
    public class EmailNotification
    {
        public Guid Id { get; set; }
        public Guid MessageTemplateId { get; set; }
        public MessageTemplate MessageTemplate { get; set; } = default!;
        [Required]
        [EmailAddress]
        [MaxLength(320)]
        public required string To { get; set; }
        [Required]
        [MaxLength(256)]
        public required string Subject { get; set; }
        [Required]
        [MaxLength(4000)]
        public required string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public NotificationStatus Status { get; set; }
        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }
        public int RetryCount { get; set; }
        public ICollection<EmailNotificationResource> Resources { get; set; } = [];
    }
}
