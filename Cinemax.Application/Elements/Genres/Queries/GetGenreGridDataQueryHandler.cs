using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Genres;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Genres.Queries
{
    public class GetGenreGridDataQueryHandler(IGenreRepository repository) : IRequestHandler<GetGenreGridDataQuery, GridResponse<GenreDto>>
    {
        private readonly IGenreRepository _repository = repository;
        public async Task<GridResponse<GenreDto>> Handle(GetGenreGridDataQuery query, CancellationToken ct)
        {
            var pageNumber = query.Request.PageNumber;
            var pageSize = query.Request.PageSize;

            var totalCount = await _repository.CountAsync(ct);
            var items = await _repository.GetPaged(pageNumber, pageSize)
                .Select(g => new GenreDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsActive = g.IsActive
                }).ToListAsync(ct);


            return new GridResponse<GenreDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
