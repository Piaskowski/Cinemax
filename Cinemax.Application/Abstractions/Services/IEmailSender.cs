
using Cinemax.Shared.Contracts.Notifications;
using Cinemax.Shared.Enums;

namespace Cinemax.Application.Abstractions.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, EmailTemplateType type, string body, IEnumerable<NotificationResource>? resources, CancellationToken ct);
    }
}
