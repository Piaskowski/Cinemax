
namespace Cinemax.Domain.Entities.Notifications
{
    public class EmailNotificationResource
    {
        public Guid Id { get; set; }

        public Guid EmailNotificationId { get; set; }
        public EmailNotification EmailNotification { get; set; } = default!;

        public required string ContentId { get; set; }
        public required string FileName { get; set; }
        public required byte[] Content { get; set; }
    }
}
