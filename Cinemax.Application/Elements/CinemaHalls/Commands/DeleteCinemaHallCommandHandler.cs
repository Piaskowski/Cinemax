using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.CinemaHalls.Commands
{
    public class DeleteCinemaHallCommandHandler(ICinemaHallRepository halls,
        IScreeningRepository screenings) : IRequestHandler<DeleteCinemaHallCommand>
    {
        private readonly ICinemaHallRepository _halls = halls;
        private readonly IScreeningRepository _screenings = screenings;
        public async Task Handle(DeleteCinemaHallCommand command, CancellationToken ct)
        {
            var existsConenction = await _screenings.Query()
                .AnyAsync(s => s.CinemaHallId == command.Id, ct);

            if (existsConenction)
                throw new ValidationException([
                    ValidationMessages.Cinemahall_AssignedScreenings
                ]);


            await _halls.DeleteAsync(command.Id, ct);
        }
    }
}
