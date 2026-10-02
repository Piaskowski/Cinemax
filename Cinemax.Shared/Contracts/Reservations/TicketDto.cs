using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Reservations
{
    public class TicketDto
    {
        public TicketType TicketType { get; set; }
        public SeatType SeatType { get; set; }
        public decimal Price { get; set; }
    }
}
