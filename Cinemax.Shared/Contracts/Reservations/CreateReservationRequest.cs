namespace Cinemax.Shared.Contracts.Reservations
{
    public class CreateReservationRequest
    {
        public Guid SeatId { get; set; }
        public Guid TicketPriceId { get; set; }
    }
}
