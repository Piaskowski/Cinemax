using Cinemax.Shared.Contracts.Scrennings;
using MediatR;

namespace Cinemax.Application.Elements.Screenings.Queries
{
    public record GetSreeningSeatsQuery(Guid Id) : IRequest<GetScreeningSeatsResponse>;
}
