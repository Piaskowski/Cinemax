using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Resources.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<ResetPasswordCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task Handle(ResetPasswordCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user is not null)
            {
                string token;

                try
                {
                    token = Encoding.UTF8.GetString(
                        WebEncoders.Base64UrlDecode(request.Token));
                }catch (FormatException)
                {
                    throw new ValidationException([AuthMessages.Error_InvalidResetPasswordUrl]);
                }

                var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

                if (!result.Succeeded) {
                    throw new ValidationException(result.Errors.Select(x => x.Description));
                }
            }
        }
    }
}
