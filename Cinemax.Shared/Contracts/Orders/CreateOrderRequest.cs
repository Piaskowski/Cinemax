using Cinemax.Shared.Contracts.Reservations;

namespace Cinemax.Shared.Contracts.Orders
{
    public class CreateOrderRequest
    {
        public Guid ScreeningId { get; set; }
        public IEnumerable<CreateReservationRequest> Reservations { get; set; } = [];
    }
}
