using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.Notifications.Repositories;
using Cinemax.Domain.Constants;
using Cinemax.Shared.Contracts.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Notifications.Commands
{
    public class SendPendingEmailNotificationsCommandHandler(IEmailNotificationRepository emails,
        IEmailSender emailSender) : IRequestHandler<SendPendingEmailNotificationsCommand>
    {
        private readonly IEmailNotificationRepository _emails = emails;
        private readonly IEmailSender _emailSender = emailSender;
        public async Task Handle(SendPendingEmailNotificationsCommand command, CancellationToken ct)
        {
            var emails = await _emails.Query()
                .Include(e => e.Resources)
                .Include(e => e.MessageTemplate)
                .Where(e => e.Status == NotificationStatus.Pending)
                .OrderBy(e => e.CreatedAt)
                .Take(20)
                .ToListAsync();
            foreach (var email in emails) 
            {
                try
                {
                    await _emailSender.SendEmailAsync(
                            email.To,
                            email.Subject,
                            email.MessageTemplate.Type,
                            email.Body,
                            email.Resources.Select(r => new NotificationResource
                            {
                                ContentId = r.ContentId,
                                Content = r.Content,
                                FileName = r.FileName
                            }),
                            ct
                        );

                    email.Status = NotificationStatus.Sent;
                    email.SentAt = DateTime.UtcNow;
                }
                catch (Exception ex) {
                    email.RetryCount++;
                    email.ErrorMessage = ex.Message;

                    if(email.RetryCount >= 3)
                    {
                        email.Status = NotificationStatus.Cancelled;
                    }
                }

                await _emails.UpdateAsync(email, ct);
            }
        }
    }
}
