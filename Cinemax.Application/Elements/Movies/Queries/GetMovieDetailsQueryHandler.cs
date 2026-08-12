using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Shared.Contracts.Genres;
using Cinemax.Shared.Contracts.Movies;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Movies.Queries
{
    public class GetMovieDetailsQueryHandler(IMovieRepository movies) : IRequestHandler<GetMovieDetailsQuery, GetMovieDetailsResponse>
    {
        private readonly IMovieRepository _movies = movies;
        public async Task<GetMovieDetailsResponse> Handle(GetMovieDetailsQuery query, CancellationToken ct)
        {
            var movie = await _movies.Query()
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == query.Id) ??
                    throw new NotFoundException(ValidationMessages.NotFound_Movie);

            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Director = movie.Director,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                PosterUrl = movie.PosterUrl,
                TrailerUrl = movie.TrailerUrl,
                Genres = [.. movie.Genres.Select(g => new GenreDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsActive = g.IsActive,
                })],
            };

            return new GetMovieDetailsResponse
            {
                MovieDto = movieDto
            };
        }
    }
}
