using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.Orders.Services;
using Cinemax.Application.Exceptions;
using Cinemax.Shared.Contracts.Orders;
using Cinemax.Shared.Contracts.Reservations;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Cinemax.Application.Elements.Orders.Queries
{
    public class GetUserOrderDetailsQueryHandler(IOrderService orderService,
        IQrCodeService qrCodeService,
        IConfiguration configuration) : IRequestHandler<GetUserOrderDetailsQuery, GetUserOrderDetailsQueryResponse>
    {
        private readonly IOrderService _orderService = orderService;
        private readonly IQrCodeService _qrCodeService = qrCodeService;
        private readonly string _domain = configuration["App:Url"]
            ?? throw new InvalidOperationException(
                ValidationMessages.Error_AppUrlNotConfigured);
        public async Task<GetUserOrderDetailsQueryResponse> Handle(GetUserOrderDetailsQuery query, CancellationToken ct)
        {
            var order = await _orderService.GetUserOrder(query.Id, ct) ??
                throw new NotFoundException(ValidationMessages.NotFound_Order);

            var qrPng = _qrCodeService.Generate($"{_domain}/check-in/{order.QrToken}"); 
            return new GetUserOrderDetailsQueryResponse
            {
                CinemaHallNumber = order.Screening.CinemaHall.Number,
                MovieTitle = order.Screening.Movie.Title,
                DurationMinutes = order.Screening.Movie.DurationMinutes,
                PosterUrl = order.Screening.Movie.PosterUrl!,
                StartTime = order.Screening.StartTime,
                QrPng = qrPng,
                Status = order.Status,
                Reservations = order.Reservations.Select(r => new OrderDetailsReservationDto
                {
                    SeatType = r.Seat.Type,
                    TicketType = r.TicketType
                })
            };

        }
    }
}
