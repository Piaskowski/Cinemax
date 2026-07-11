using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinemax.Application.Elements.Orders.Queries
{
    public class GetOrderGridDataQueryHandler(IOrderRepository repository,
        ILogger<GetOrderGridDataQueryHandler> logger) : IRequestHandler<GetOrderGridDataQuery, GridResponse<OrderDto>>
    {
        private readonly IOrderRepository _repository = repository;
        private readonly ILogger _logger = logger;
        public async Task<GridResponse<OrderDto>> Handle(GetOrderGridDataQuery query, CancellationToken cancellationToken)
        {
            var pageNumber = query.Request.PageNumber;
            var pageSize = query.Request.PageSize;

            var totalCount = await _repository.CountAsync(cancellationToken);
            var items = await _repository.GetPaged(pageNumber, pageSize)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerEmail = o.CustomerEmail,
                    CreatedByUserEmail = o.CreatedByUser != null ? o.CreatedByUser.Email : null,
                    ScreeningId = o.ScreeningId,
                    ScreeningDate = o.Screening.StartTime,
                    ReservationCount = o.Reservations.Count,
                    Status = o.Status,
                    Source = o.Source,
                    CreatedAt = o.CreatedAt,
                    ExpiresAt = o.ExpiresAt
                }).ToListAsync(cancellationToken: cancellationToken);


            return new GridResponse<OrderDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
