using Cinemax.Shared.Contracts.CinemaHalls;
using MediatR;

namespace Cinemax.Application.Elements.CinemaHalls.Commands
{
    public record EditCinemaHallCommand(EditCinemaHallRequest Request) : IRequest;
}
