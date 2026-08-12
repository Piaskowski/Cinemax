using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                throw new ValidationException([ValidationMessages.Cinemhall_UnavailableNumber]); 

            var newCinemaHall = new CinemaHall
            {
                Number = request.Number,
                Type = request.Type,
            };

            await _repository.CreateAsync(newCinemaHall, ct);
        }
    }
}
