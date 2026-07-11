using Cinemax.Application.Elements.Tickets.Repositories;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.TicketPrices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinemax.Application.Elements.Tickets.Queries
{
    public class GetTicketPriceGridDataQueryHandler(ITicketRepository repository,
        ILogger<GetTicketPriceGridDataQuery> logger) : IRequestHandler<GetTicketPriceGridDataQuery, GridResponse<TicketPriceDto>>
    {
        private readonly ITicketRepository _repository = repository;
        private readonly ILogger _logger = logger;
        public async Task<GridResponse<TicketPriceDto>> Handle(GetTicketPriceGridDataQuery query, CancellationToken cancellationToken)
        {
            var pageNumber = query.Request.PageNumber;
            var pageSize = query.Request.PageSize;

            var totalCount = await _repository.CountAsync(cancellationToken);
            var items = await _repository.GetPaged(pageNumber, pageSize)
                .Select(t => new TicketPriceDto
                {
                    Id = t.Id,
                    TicketType = t.TicketType,
                    ScreeningType = t.ScreeningType,
                    Price = t.Price,
                    IsActive = t.IsActive
                }).ToListAsync(cancellationToken: cancellationToken);


            return new GridResponse<TicketPriceDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
