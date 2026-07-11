using Cinemax.Domain.Constants;

namespace Cinemax.Domain.Entities.Orders
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public Guid SeatId { get; set; }
        public Seat Seat { get; set; } = default!;
        public ReservationStatus Status { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
