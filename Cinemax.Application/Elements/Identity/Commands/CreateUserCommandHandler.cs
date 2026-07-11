using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Contracts.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class CreateUserCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<CreateUserResponse> Handle(CreateUserCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
                throw new ValidationException(["Użytkownik o podanym adresie email już istnieje."]);

            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                IsActive = false
            };

            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded)
                throw new ValidationException(
                    result.Errors.Select(x => x.Description));

            await _userManager.AddToRoleAsync(newUser, request.Role.ToString());

            return new CreateUserResponse
            {
                Message = "Użytkownik został utworzony"
            };
        }
    }
}
