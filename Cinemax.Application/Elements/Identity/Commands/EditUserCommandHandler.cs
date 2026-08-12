using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Resources.Common;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class EditUserCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<EditUserCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task Handle(EditUserCommand command, CancellationToken ct)
        {
            var request = command.Request;
            var normalizedEmail = request.Email.Trim().ToUpper();

            var exists = await _userManager.Users
                .AnyAsync(u =>
                    u.Email!.ToUpper().Equals(normalizedEmail) &&
                    u.Id != request.Id, ct);

            if (exists)
                throw new ValidationException([ValidationMessages.User_UnavailableEmail]);

            var editedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id, ct) ??
                throw new ValidationException([CommonMessages.Error_OperationFailed]);

            editedUser.FirstName = request.FirstName;
            editedUser.LastName = request.LastName;
            editedUser.Email = request.Email;
            editedUser.IsActive = request.IsActive;

            var roles = await _userManager.GetRolesAsync(editedUser);
            if (!roles.Contains(request.Role.ToString()))
            {
                await _userManager.RemoveFromRolesAsync(editedUser, roles);
                await _userManager.AddToRoleAsync(editedUser, request.Role.ToString());
            }

            await _userManager.UpdateAsync(editedUser);
        }
    }
}
