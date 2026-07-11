using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Movies.Commands
{
    public class CreateMovieCommandHandler(IMovieRepository movieRepository, IGenreRepository genreRepository) : IRequestHandler<CreateMovieCommand>
    {
        private readonly IMovieRepository _movieRepository = movieRepository;
        private readonly IGenreRepository _genreRepository = genreRepository;

        public async Task Handle(CreateMovieCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var normalizedTitle = request.Title.Trim().ToUpper();

            var exists = await _movieRepository.Query()
                .AnyAsync(m => m.Title.ToUpper() == normalizedTitle, ct);

            if (exists)
                throw new ValidationException([$"Film o tytule \"{request.Title}\" już istnieje."]);

            var genres = await _genreRepository.Query()
                .Where(g => request.GenresIds.Contains(g.Id))
                .ToListAsync(ct);

            var newMovie = new Movie
            {
                Title = request.Title,
                Director = request.Director,
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                Genres = genres,
                PosterUrl = request.PosterUrl,
                TrailerUrl = request.TrailerUrl,
            };

            await _movieRepository.CreateAsync(newMovie);
        }
    }
}
