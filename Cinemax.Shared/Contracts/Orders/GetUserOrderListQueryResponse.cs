
namespace Cinemax.Shared.Contracts.Orders
{
    public class GetUserOrderListQueryResponse
    {
        public List<OrderItemDto> Items { get; set; } = [];
        public int TotalPages { get; set; }
    }
}
