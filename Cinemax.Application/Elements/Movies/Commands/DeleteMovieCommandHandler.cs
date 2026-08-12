using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Movies.Commands
{
    public class DeleteMovieCommandHandler(IMovieRepository movies,
        IScreeningRepository screenings) : IRequestHandler<DeleteMovieCommand>
    {
        private readonly IMovieRepository _movies = movies;
        private readonly IScreeningRepository _screenings = screenings;
        public async Task Handle(DeleteMovieCommand command, CancellationToken ct)
        {
            var hasScreenings = await _screenings.Query()
                .AnyAsync(s => s.MovieId == command.Id);

            if (hasScreenings)
                throw new ValidationException([ValidationMessages.Movie_AssignedScreenings]);

            await _movies.DeleteAsync(command.Id, ct);
        }
    }
}
