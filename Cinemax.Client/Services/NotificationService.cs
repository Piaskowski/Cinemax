namespace Cinemax.Client.Services
{
    public class NotificationService
    {
        public event Action<NotificationMessage>? OnShow;

        public void Success(string message)
        {
            OnShow?.Invoke(new NotificationMessage(message, NotificationType.Success));
        }

        public void Error(string message)
        {
            OnShow?.Invoke(new NotificationMessage(message, NotificationType.Error));
        }

        public void Warning(string message)
        {
            OnShow?.Invoke(new NotificationMessage(message, NotificationType.Warning));
        }

        public void Info(string message)
        {
            OnShow?.Invoke(new NotificationMessage(message, NotificationType.Info));
        }
    }

    public record NotificationMessage(string Message, NotificationType Type);

    public enum NotificationType
    {
        Success,
        Error,
        Warning,
        Info
    }
}
