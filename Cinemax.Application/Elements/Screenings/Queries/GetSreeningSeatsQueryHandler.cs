using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Constants;
using Cinemax.Shared.Contracts.Scrennings;
using Cinemax.Shared.Contracts.Seats;
using Cinemax.Shared.Enums;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Screenings.Queries
{
    public class GetSreeningSeatsQueryHandler(IScreeningRepository screenings,
        IOrderRepository orders) : IRequestHandler<GetSreeningSeatsQuery, GetScreeningSeatsResponse>
    {
        private readonly IScreeningRepository _screenings = screenings;
        private readonly IOrderRepository _orders = orders;
        public async Task<GetScreeningSeatsResponse> Handle(GetSreeningSeatsQuery query, CancellationToken ct)
        {
            var result = await _screenings.Query()
                .AsNoTracking()
                .Where(s =>
                    s.Id == query.Id &&
                    s.Status == ScreeningStatus.Scheduled)
                .Select(s => new GetScreeningSeatsResponse
                {
                    CinemaHallNumber = s.CinemaHall.Number,
                    MovieTitle = s.Movie.Title,
                    PosterUrl = s.Movie.PosterUrl!,
                    DurationMinutes = s.Movie.DurationMinutes,
                    StartTime = s.StartTime,

                    SeatsRows = s.CinemaHall.Seats
                        .Where(seat => seat.IsActive)
                        .GroupBy(seat => seat.Row)
                        .Select(group => new ScreeningSeatsRowDto
                        {
                            Row = group.Key,
                            Seats = group
                                .OrderBy(seat => seat.Number)
                                .Select(seat => new ScreeningSeatDto
                                {
                                    Id = seat.Id,
                                    Row = seat.Row,
                                    Number = seat.Number,
                                    Type = seat.Type
                                })
                                .ToList()
                        })
                        .OrderBy(row => row.Row)
                        .ToList()
                }).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(ValidationMessages.Error_NoResults);

            // == Collecting booked/sold seats for the given screening ==
            var ignoredStatuses = new[]
            {
                OrderStatus.Cancelled,
                OrderStatus.Expired
            };

            var reservations = await _orders.Query()
                .AsNoTracking()
                .Where(o => o.ScreeningId == query.Id && !ignoredStatuses.Contains(o.Status))
                .SelectMany(o => o.Reservations)
                .Where(r => r.Status != ReservationStatus.Cancelled)
                .Select(r => new
                {
                    r.SeatId,
                    r.Status
                })
                .ToListAsync(ct);

            var reservationsBySeatId = reservations
               .ToDictionary(r => r.SeatId, r => r.Status);

            // == Updating seat statuses ==
            foreach (var row in result.SeatsRows)
            {
                foreach (var seat in row.Seats)
                {
                    if (!reservationsBySeatId.TryGetValue(
                            seat.Id,
                            out var reservationStatus))
                    {
                        seat.Status = ScreeningSeatStatus.Available;
                        continue;
                    }

                    seat.Status = reservationStatus switch
                    {
                        ReservationStatus.Pending => ScreeningSeatStatus.Booked,
                        ReservationStatus.Confirmed => ScreeningSeatStatus.Sold,
                        _ => ScreeningSeatStatus.Available
                    };
                }
            }

            return result;
        }
    }
}
