using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Resources.Common;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class DeleteUserCommandHandler(UserManager<ApplicationUser> userManager,
        IOrderRepository orders) : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IOrderRepository _orders = orders;
        public async Task Handle(DeleteUserCommand command, CancellationToken ct)
        {
            var hasOrders = await _orders.Query()
                .AnyAsync(o => o.UserId == command.Id || o.CreatedByUserId == command.Id);

            if (hasOrders)
                throw new ValidationException([ValidationMessages.User_AssignedOrders]);

            var user = await _userManager.FindByIdAsync(command.Id.ToString()) ??
                throw new ValidationException([CommonMessages.Error_OperationFailed]);

            await _userManager.DeleteAsync(user);
        }
    }
}
