using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Shared.Contracts.Movies;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Movies.Queries
{
    public class GetMoviesSelectListQueryHandler(IMovieRepository movies) : IRequestHandler<GetMoviesSelectListQuery, IEnumerable<MovieSelectItemDto>>
    {
        private readonly IMovieRepository _movies = movies;
        public async Task<IEnumerable<MovieSelectItemDto>> Handle(GetMoviesSelectListQuery query, CancellationToken ct)
        {
            return await _movies.Query()
                .Select(m => new MovieSelectItemDto { 
                    Id = m.Id,
                    Title = m.Title
                }).ToListAsync(ct);
        }
    }
}
