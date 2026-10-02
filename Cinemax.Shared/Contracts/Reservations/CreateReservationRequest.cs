using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Reservations
{
    public class CreateReservationRequest
    {
        public Guid SeatId { get; set; }
        public TicketType TicketType { get; set; }
    }
}
