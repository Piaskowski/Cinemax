
namespace Cinemax.Shared.Contracts.Notifications
{
    public class NotificationResource
    {
        public required string ContentId { get; set; }
        public required string FileName { get; set; }
        public required byte[] Content { get; set; }
    }
}
