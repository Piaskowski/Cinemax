using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Reservations
{
    public class EditOrderReservationRequest
    {
        public Guid Id { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
