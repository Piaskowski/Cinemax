using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.Notifications.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Constants;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Domain.Entities.Notifications;
using Cinemax.Shared.Contracts.Identity;
using Cinemax.Shared.Resources.Auth;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class CreateUserCommandHandler(UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IEmailMessageRepository templates,
        IEmailNotificationRepository notifications,
        IEmailBodyRenderer bodyRenderer) : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IEmailMessageRepository _templates = templates;
        private readonly IEmailNotificationRepository _notifications = notifications;
        private readonly IEmailBodyRenderer _bodyRenderer = bodyRenderer;
        private readonly string _domain = configuration["App:Url"]
            ?? throw new InvalidOperationException(
                ValidationMessages.Error_AppUrlNotConfigured);
        public async Task<CreateUserResponse> Handle(CreateUserCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
                throw new ValidationException([ValidationMessages.User_UnavailableEmail]);

            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded)
                throw new ValidationException(
                    result.Errors.Select(x => x.Description));

            await _userManager.AddToRoleAsync(newUser, request.Role.ToString());

            var emailtoken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(newUser);

            await GenerateCompleteRegistrationNotification(newUser, emailtoken, passwordToken, ct);

            return new CreateUserResponse
            {
                Message = AuthMessages.CreateSuccess
            };
        }

        private async Task GenerateCompleteRegistrationNotification(ApplicationUser user, string emailToken, string passwordToken, CancellationToken ct)
        {
            var template = await _templates.GetByCode("CompleteRegistration") ??
                throw new InvalidOperationException(string.Format(ValidationMessages.Error_EmailTemplateNotFound, "CompleteRegistration"));

            var encodedEmailToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(emailToken));

            var encodedPasswordToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(passwordToken));

            var activationLink = $"{_domain}/complete-registration" +
                $"?userId={user.Id}&emailToken={encodedEmailToken}&passwordToken={encodedPasswordToken}";

            var values = new Dictionary<string, string>
            {
                ["FirstName"] = user.FirstName,
                ["CompleteRegistration"] = activationLink
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
