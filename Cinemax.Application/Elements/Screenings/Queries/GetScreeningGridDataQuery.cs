using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Scrennings;
using MediatR;

namespace Cinemax.Application.Elements.Screenings.Queries
{
    public record GetScreeningGridDataQuery(GridRequest Request) : IRequest<GridResponse<ScreeningDto>>;
}
