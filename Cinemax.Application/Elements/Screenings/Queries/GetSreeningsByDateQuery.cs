using Cinemax.Shared.Contracts.Scrennings;
using MediatR;

namespace Cinemax.Application.Elements.Screenings.Queries
{
    public record GetSreeningsByDateQuery(DateOnly Date, Guid? MovieId) : IRequest<GetScreeningsByDateResponse>;
}
