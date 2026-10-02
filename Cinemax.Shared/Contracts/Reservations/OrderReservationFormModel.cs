using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Reservations
{
    public class OrderReservationFormModel
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int SeatNum { get; set; }
        public SeatType SeatType { get; set; }
        public TicketType TicketType { get; set; }
        public ReservationStatus Status { get; set; }
        public bool CanEditStatus { get; set; }
    }
}
