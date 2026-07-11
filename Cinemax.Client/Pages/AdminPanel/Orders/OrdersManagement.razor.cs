using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Orders;
using Cinemax.Shared.Contracts.TicketPrices;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Orders
{
    public partial class OrdersManagement
    {
        private DataGrid<OrderDto>? _ordersGrid;
        private async Task<GridResponse<OrderDto>> LoadData(GridRequest request)
        {
            // TODO : Dodać numer zamówienia, sumę zamówienia
            var response = await HttpClient.PostAsJsonAsync("api/admin/Orders", request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<OrderDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<OrderDto>>() ?? new GridResponse<OrderDto>();
        }

        private async Task RefreshDataAsync()
        {
            await _ordersGrid!.ReloadAsync();
        }
    }
}
