using Cinemax.Shared.Contracts.Reservations;

namespace Cinemax.Shared.Contracts.Orders
{
    public class NewOrderRequest
    {
        public Guid ScreeningId { get; set; }
        public string? CustomerEmail { get; set; }
        public IEnumerable<CreateReservationRequest> Reservations { get; set; } = [];
    }
}
