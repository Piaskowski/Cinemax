using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Identity;
using MediatR;

namespace Cinemax.Application.Elements.Identity.Queries
{
    public record GetUserGridDataQuery(int PageNumber, int PageSize) : IRequest<GridResponse<UserDto>>;
}
