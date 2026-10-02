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
    public class ActivateAccountCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<ActivateAccountCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<bool> Handle(ActivateAccountCommand command, CancellationToken ct)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, ct) ??
                throw new ValidationException([ValidationMessages.Error_InvalidActivationUrl]);

            string token;

            try
            {
                token = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(command.Token));
            }
            catch (FormatException)
            {
                throw new ValidationException([ValidationMessages.Error_InvalidActivationUrl]);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(x => x.Description));
            }

            return true;
        }

    }
}
