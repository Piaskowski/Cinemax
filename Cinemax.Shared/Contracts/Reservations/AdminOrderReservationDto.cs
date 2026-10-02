using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Reservations
{
    public class AdminOrderReservationDto
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int SeatNum { get; set; }
        public TicketType TicketType { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
