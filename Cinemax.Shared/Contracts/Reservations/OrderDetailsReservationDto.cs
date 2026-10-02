
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Reservations
{
    public class OrderDetailsReservationDto
    {
        public TicketType TicketType { get; set; }
        public SeatType SeatType { get; set; }
    }
}
