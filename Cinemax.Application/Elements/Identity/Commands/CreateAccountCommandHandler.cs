using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.Notifications.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Constants;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Domain.Entities.Notifications;
using Cinemax.Shared.Contracts.Auth;
using Cinemax.Shared.Enums;
using Cinemax.Shared.Resources.Auth;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class CreateAccountCommandHandler(UserManager<ApplicationUser> userManager,
        IEmailMessageRepository templates,
        IEmailNotificationRepository notifications,
        IConfiguration configuration,
        IEmailBodyRenderer bodyRenderer) : IRequestHandler<CreateAccountCommand, RegisterResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IEmailMessageRepository _templates = templates;
        private readonly IEmailNotificationRepository _notifications = notifications;
        private readonly IEmailBodyRenderer _bodyRenderer = bodyRenderer;
        private readonly string _domain = configuration["App:Url"]
            ?? throw new InvalidOperationException(
                ValidationMessages.Error_AppUrlNotConfigured);
        public async Task<RegisterResponse> Handle(CreateAccountCommand command, CancellationToken ct)
        {

            var user = new ApplicationUser
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                UserName = command.Email,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, command.Password);

            // TODO: wykonać tłumaczenia dla Identity
            if(!result.Succeeded) 
                throw new ValidationException(
                    result.Errors.Select(x => x.Description));

            await _userManager.AddToRoleAsync(user, UserRole.Customer.ToString());

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            await GenerateEmailConfirmationNotification(user, encodedToken, ct);

            return new RegisterResponse {
                Message = AuthMessages.Info_AccountSuccessfullyCreated,
            };
        }

        private async Task GenerateEmailConfirmationNotification(ApplicationUser user, string encodedToken, CancellationToken ct)
        {
            var template = await _templates.GetByCode("EmailConfirmation") ?? 
                throw new InvalidOperationException(string.Format(ValidationMessages.Error_EmailTemplateNotFound, "EmailConfirmation"));

            var activationLink = $"{_domain}/confirm-email" +
                $"?userId={user.Id}&token={encodedToken}";

            var values = new Dictionary<string, string>
            {
                ["FirstName"] = user.FirstName,
                ["ActivationLink"] = activationLink
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
