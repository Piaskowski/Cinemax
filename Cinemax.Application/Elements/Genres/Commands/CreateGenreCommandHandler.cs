using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Genres.Commands
{
    internal class CreateGenreCommandHandler(IGenreRepository repository) : IRequestHandler<CreateGenreCommand>
    {
        private readonly IGenreRepository _repository = repository;
        public async Task Handle(CreateGenreCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var normalizedName = request.Name.Trim().ToUpper();

            var exists = await _repository.Query()
                .AnyAsync(g => g.Name.ToUpper().Equals(normalizedName), ct);

            if (exists)
                throw new ValidationException([$"Kategoria o nazwie \"{request.Name}\" już istnieje."]);

            var newGenre = new Genre { Name = request.Name };
            await _repository.CreateAsync(newGenre, ct);
        }
    }
}
