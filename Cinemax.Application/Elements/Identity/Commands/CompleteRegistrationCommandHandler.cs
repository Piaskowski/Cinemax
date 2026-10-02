using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class CompleteRegistrationCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<CompleteRegistrationCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task Handle(CompleteRegistrationCommand command, CancellationToken ct)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, ct) ??
                throw new ValidationException([ValidationMessages.Error_InvalidActivationUrl]);

            string emailToken;
            string passwordToken;

            try
            {
                emailToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(command.EmailToken));

                passwordToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(command.PasswordToken));
            }
            catch (FormatException)
            {
                throw new ValidationException([ValidationMessages.Error_InvalidActivationUrl]);
            }

            var result = await _userManager.ConfirmEmailAsync(user, emailToken);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(x => x.Description));
            }

            result = await _userManager.ResetPasswordAsync(user, passwordToken, command.Password);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(x => x.Description));
            }
        }
    }
}
