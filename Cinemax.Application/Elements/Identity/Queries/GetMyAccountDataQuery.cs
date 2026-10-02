using Cinemax.Shared.Contracts.User;
using MediatR;

namespace Cinemax.Application.Elements.Identity.Queries
{
    public record GetMyAccountDataQuery : IRequest<GetMyAccountDataResponse>;
}
