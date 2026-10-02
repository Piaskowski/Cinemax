
using Cinemax.Application.Abstractions.Services;
using Cinemax.Shared.Contracts.Notifications;
using Cinemax.Shared.Enums;
using Cinemax.Shared.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Cinemax.Mailing.MailKit
{
    public class MailKitEmailSender(IOptions<MailingSettings> settings) : IEmailSender
    {
        private readonly MailingSettings _settings = settings.Value;

        public async Task SendEmailAsync(string to, string subject, EmailTemplateType type, string body, IEnumerable<NotificationResource>? resources, CancellationToken ct)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_settings.Name, _settings.Email));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;


            if (type == EmailTemplateType.Html) 
            {
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };

                if (resources is not null)
                {
                    foreach (var resource in resources)
                    {
                        var linkedResoure = bodyBuilder.LinkedResources.Add(
                                resource.FileName,
                                resource.Content
                            );

                        linkedResoure.ContentId = resource.ContentId;
                    }
                }

                email.Body = bodyBuilder.ToMessageBody();
            }
            else
            {
                email.Body = new TextPart(TextFormat.Text) { Text = body };
            }
                

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password, ct);
            await smtp.SendAsync(email, ct);
            await smtp.DisconnectAsync(true, ct);
        }
    }
}
