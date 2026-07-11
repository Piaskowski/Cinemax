using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Cinemax.Application.Elements.Genres.Commands
{
    public class EditGenreCommandHandler(IGenreRepository genres) : IRequestHandler<EditGenreCommand>
    {
        private readonly IGenreRepository _genres = genres;

        public async Task Handle(EditGenreCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var normalizedName = request.Name.Trim().ToUpper();

            var exists = await _genres.Query()
                .AnyAsync(g => 
                g.Name.ToUpper().Equals(normalizedName) && 
                !g.Id.Equals(request.Id), ct);

            if (exists)
                throw new ValidationException([$"Kategoria o nazwie \"{request.Name}\" już istnieje."]);

            var editedGenre = await _genres.GetByIdAsync(request.Id, ct) ?? 
                throw new ValidationException([$"Kategoria nie istnieje."]);

            editedGenre.Name = request.Name;
            editedGenre.IsActive = request.IsActive;
            await _genres.UpdateAsync(editedGenre, ct);
        }
    }
}
