using Cinemax.Shared.Contracts.Reservations;
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Orders
{
    public class GetUserOrderDetailsQueryResponse
    {
        public int CinemaHallNumber { get; set; } = default!;
        public string MovieTitle { get; set; } = default!;
        public int DurationMinutes { get; set; }
        public string PosterUrl { get; set; } = default!;
        public DateTime StartTime { get; set; } = default!;
        public byte[]? QrPng { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderDetailsReservationDto> Reservations { get; set; } = [];
    }
}
