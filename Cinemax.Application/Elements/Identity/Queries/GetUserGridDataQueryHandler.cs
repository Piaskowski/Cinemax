using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Identity;
using Cinemax.Shared.Enums;
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

            var users = await _userManager.Users
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(ct);


            var items = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var roleName = roles.FirstOrDefault();

                var role = Enum.TryParse<UserRole>(roleName, out var parsedRole)
                    ? parsedRole
                    : default;

                items.Add(new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Role = role,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    ModifiedAt = user.ModifiedAt,
                    ModifiedBy = user.ModifiedBy,
                });
            }

            return new GridResponse<UserDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
