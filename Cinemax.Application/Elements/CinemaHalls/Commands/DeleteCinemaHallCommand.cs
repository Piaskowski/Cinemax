using MediatR;

namespace Cinemax.Application.Elements.CinemaHalls.Commands
{
    public record DeleteCinemaHallCommand(Guid Id) : IRequest;
}
