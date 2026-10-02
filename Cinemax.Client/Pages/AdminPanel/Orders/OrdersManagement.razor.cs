using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Orders;
using Cinemax.Shared.Contracts.Reservations;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Orders
{
    public partial class OrdersManagement
    {
        private DataGrid<OrderDto>? _ordersGrid;
        private readonly string _editOrderDialogId = "edit-order-modal";
        private OrderEditFormModel _selectedOrder = new();
        private async Task<GridResponse<OrderDto>> LoadData(GridRequest request)
        {
            // TODO : Dodać numer zamówienia, sumę zamówienia
            var response = await HttpClient.PostAsJsonAsync("api/admin/Orders", request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<OrderDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<OrderDto>>() ?? new GridResponse<OrderDto>();
        }

        private void SelectOrder(OrderDto order)
        {
            _selectedOrder = new OrderEditFormModel
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                CustomerEmail = order.CustomerEmail,
                TotalPrice = order.TotalPrice,
                ScreeningDate = order.ScreeningDate,
                CreatedByUserEmail = order.CreatedByUserEmail,
                Status = order.Status,
                CanEditStatus = order.Status == Cinemax.Shared.Enums.OrderStatus.Pending,
                Reservations = [.. order.Reservations.Select(r => new OrderReservationFormModel
                {
                    Id = r.Id,
                    Status = r.Status,
                    CanEditStatus = r.Status == Cinemax.Shared.Enums.ReservationStatus.Pending,
                    SeatNum = r.SeatNum,
                    Row = r.Row,
                    TicketType = r.TicketType,
                })],
            };
        }

        private async Task RefreshDataAsync()
        {
            await _ordersGrid!.ReloadAsync();
        }
    }
}
