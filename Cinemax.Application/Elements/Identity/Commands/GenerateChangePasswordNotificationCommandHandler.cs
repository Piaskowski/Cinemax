using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.Notifications.Repositories;
using Cinemax.Domain.Constants;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Domain.Entities.Notifications;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class GenerateChangePasswordNotificationCommandHandler(UserManager<ApplicationUser> userManager,
        IEmailMessageRepository templates,
        IEmailNotificationRepository notifications,
        IConfiguration configuration,
        IEmailBodyRenderer bodyRenderer) : IRequestHandler<GenerateChangePasswordNotificationCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IEmailMessageRepository _templates = templates;
        private readonly IEmailNotificationRepository _notifications = notifications;
        private readonly IEmailBodyRenderer _bodyRenderer = bodyRenderer;
        private readonly string _domain = configuration["App:Url"]
            ?? throw new InvalidOperationException(
                ValidationMessages.Error_AppUrlNotConfigured);

        public async Task Handle(GenerateChangePasswordNotificationCommand command, CancellationToken ct)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email!.Equals(command.Email), ct);

            if (user is not null) {

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(token));

                var template = await _templates.GetByCode("PasswordChange") ??
                    throw new InvalidOperationException(string.Format(ValidationMessages.Error_EmailTemplateNotFound, "EmailConfirmation"));


                var activationLink = $"{_domain}/change-password" +
                    $"?userId={user.Id}&token={encodedToken}";

                var values = new Dictionary<string, string>
                {
                    ["FirstName"] = user.FirstName,
                    ["PasswordChangeLink"] = activationLink
                };

                var notification = new EmailNotification
                {
                    Subject = _bodyRenderer.Render(template!.Subject, values),
                    Body = _bodyRenderer.Render(template.Body, values),
                    To = user.Email!,
                    Status = NotificationStatus.Pending,
                    MessageTemplateId = template.Id
                };

                await _notifications.CreateAsync(notification, ct);
            }
        }
    }
}
