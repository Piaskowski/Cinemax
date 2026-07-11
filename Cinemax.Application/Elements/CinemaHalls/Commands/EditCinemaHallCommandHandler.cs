
using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.CinemaHalls.Commands
{
    public class EditCinemaHallCommandHandler(ICinemaHallRepository halls) : IRequestHandler<EditCinemaHallCommand>
    {
        private readonly ICinemaHallRepository _halls= halls;
        public async Task Handle(EditCinemaHallCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var exists = await _halls.Query()
                .AnyAsync(c => c.Number == request.Number && c.Id != request.Id, ct);

            if (exists)
                throw new ValidationException([$"Sala o podanym numerze już istnieje."]);

            var editedHall = await _halls.GetByIdAsync(request.Id, ct) ??
                throw new ValidationException([$"Wskazana sala nie istnieje."]);

            editedHall.Number = request.Number;
            editedHall.Type = request.Type;
            editedHall.IsActive = request.IsActive;

            await _halls.UpdateAsync(editedHall, ct);
        }
    }
}
