
using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Cinemax.Application.Elements.CinemaHalls.Commands
{
    public class CreateCinemaHallCommandHandler(ICinemaHallRepository repository) : IRequestHandler<CreateCinemaHallCommand>
    {
        private readonly ICinemaHallRepository _repository = repository;

        public async Task Handle(CreateCinemaHallCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var exists = await _repository.Query()
                .AnyAsync(c => c.Number == request.Number, ct);

            if (exists)
                throw new ValidationException([$"Sala o podanym numerze już istnieje."]);

            var newCinemaHall = new CinemaHall
            {
                Number = request.Number,
                Type = request.Type,
            };

            await _repository.CreateAsync(newCinemaHall, ct);
        }
    }
}
