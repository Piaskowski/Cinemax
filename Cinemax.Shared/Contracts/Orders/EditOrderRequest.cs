
using Cinemax.Shared.Contracts.Reservations;
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Orders
{
    public class EditOrderRequest
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public List<EditOrderReservationRequest> Reservations { get; set; } = [];
    }
}
