using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Shared.Contracts.Genres;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Genres.Queries
{
    public class GetGenresQueryHandler(IGenreRepository repository) : IRequestHandler<GetGenresQuery, GetGenresResponse>
    {
        private readonly IGenreRepository _repository = repository;
        public async Task<GetGenresResponse> Handle(GetGenresQuery request, CancellationToken ct)
        {
            var genres = await _repository.Query()
                .Select(g => new GenreDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsActive = g.IsActive
                }).ToListAsync();

            return new GetGenresResponse
            {
                Items = genres,
            };
        }
    }
}
