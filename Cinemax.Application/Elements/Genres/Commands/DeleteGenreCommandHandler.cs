using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Genres.Commands
{
    public class DeleteGenreCommandHandler(IGenreRepository genres) : IRequestHandler<DeleteGenreCommand>
    {
        private readonly IGenreRepository _genres = genres;
        public async Task Handle(DeleteGenreCommand command, CancellationToken ct)
        {
            var hasMovies = await _genres.Query()
                .Where(g => g.Id == command.Id)
                .AnyAsync(g => g.Movies.Any(), ct);

            if (hasMovies)
                throw new ValidationException([ValidationMessages.Genre_AssignedMovies]);

            await _genres.DeleteAsync(command.Id, ct);
        }
    }
}
