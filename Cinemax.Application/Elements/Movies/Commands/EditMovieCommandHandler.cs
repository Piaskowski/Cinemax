using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Movies.Commands
{
    public class EditMovieCommandHandler(IMovieRepository movies,
        IGenreRepository genres) : IRequestHandler<EditMovieCommand>
    {
        private readonly IMovieRepository _movies = movies;
        private readonly IGenreRepository _genres = genres;
        public async Task Handle(EditMovieCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var normalizedTitle = request.Title.Trim().ToUpper();

            var exists = await _movies.Query()
                .AnyAsync(m => m.Title.ToUpper() == normalizedTitle && m.Id != request.Id, ct);

            if (exists)
                throw new ValidationException([$"Film o tytule \"{request.Title}\" już istnieje."]);

            var editedMovie = await _movies.Query()
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == request.Id) ??
                throw new ValidationException([$"Wskazany film nie istnieje."]);

            var genres = await _genres.Query()
                .Where(g => request.GenresIds.Contains(g.Id))
                .ToListAsync(ct);

            editedMovie.Genres.Clear();

            editedMovie.Title = request.Title;
            editedMovie.Director = request.Director;
            editedMovie.Description = request.Description;
            editedMovie.Genres = genres;
            editedMovie.PosterUrl = request.PosterUrl;
            editedMovie.TrailerUrl = request.TrailerUrl;
            editedMovie.IsActive = request.IsActive;

            await _movies.UpdateAsync(editedMovie, ct);
        }
    }
}
