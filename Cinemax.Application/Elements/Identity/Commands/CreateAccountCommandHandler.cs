using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Contracts.Auth;
using Cinemax.Shared.Enums;
using Cinemax.Shared.Resources.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class CreateAccountCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<CreateAccountCommand, RegisterResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<RegisterResponse> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
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
            
            return new RegisterResponse {
                Message = AuthMessages.Info_AccountSuccessfullyCreated,
            };
        }
    }
}
