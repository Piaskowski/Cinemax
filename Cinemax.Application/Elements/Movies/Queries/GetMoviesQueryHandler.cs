using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Shared.Contracts.Movies;
using Cinemax.Shared.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Cinemax.Application.Elements.Movies.Queries
{
    public class GetMoviesQueryHandler(IMovieRepository repository,
        IOptions<DisplaySettings> displaySettings) : IRequestHandler<GetMoviesQuery, GetMoviesResponse>
    {
        private readonly IMovieRepository _movies = repository;
        private readonly int _moviesPerPage = displaySettings.Value.ItemsPerPage;
        public async Task<GetMoviesResponse> Handle(GetMoviesQuery query, CancellationToken ct)
        {
            var page = query.Page > 0? query.Page : 1;

            var totalPages = (int)Math.Ceiling((double)await _movies.CountAsync(ct) / _moviesPerPage);

            var movies = await _movies.GetPaged(page, _moviesPerPage)
                .Where(m => m.IsActive)
                .Select(m => new MovieDisplayCardDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    PosterUrl = m.PosterUrl
                })
                .ToListAsync(ct);

            return new GetMoviesResponse { Movies = movies, TotalPages = totalPages };
        }
    }
}
