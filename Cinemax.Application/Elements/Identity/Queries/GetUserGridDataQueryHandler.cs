using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Identity.Queries
{
    public class GetUserGridDataQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetUserGridDataQuery, GridResponse<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<GridResponse<UserDto>> Handle(GetUserGridDataQuery query, CancellationToken ct)
        {
            var totalCount = await _userManager.Users.CountAsync(ct);
            var items = await _userManager.Users
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(u => new UserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    ModifiedAt = u.ModifiedAt,
                    ModifiedBy = u.ModifiedBy,
                }).ToListAsync(ct);

            return new GridResponse<UserDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
