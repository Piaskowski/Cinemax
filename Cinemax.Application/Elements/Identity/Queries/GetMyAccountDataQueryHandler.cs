using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Contracts.User;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Identity.Queries
{
    public class GetMyAccountDataQueryHandler(UserManager<ApplicationUser> userManager,
        ICurrentUserService currentUserService) : IRequestHandler<GetMyAccountDataQuery, GetMyAccountDataResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        public async Task<GetMyAccountDataResponse> Handle(GetMyAccountDataQuery query, CancellationToken ct)
        {
            var userId = _currentUserService.Id;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException(ValidationMessages.NotFound_User);

            return new GetMyAccountDataResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!
            };
        }
    }
}
