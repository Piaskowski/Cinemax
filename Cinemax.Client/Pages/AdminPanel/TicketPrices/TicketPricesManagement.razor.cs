using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.TicketPrices;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.TicketPrices
{
    public partial class TicketPricesManagement
    {
        private DataGrid<TicketPriceDto>? _ticketPricesGrid;
        private TicketPriceFormModel _selectedTicketPrice = new();
        private readonly string _createTicketPriceDialogId = "ticket-price-form-modal";
        private async Task<GridResponse<TicketPriceDto>> LoadData(GridRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync("api/admin/TicketPrices", request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<TicketPriceDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<TicketPriceDto>>() ?? new GridResponse<TicketPriceDto>();
        }
        private async Task RefreshDataAsync()
        {
            _selectedTicketPrice = new();
            await _ticketPricesGrid!.ReloadAsync();
        }

        private void SelectTicketPrice(TicketPriceDto ticketPrice)
        {
            _selectedTicketPrice = new TicketPriceFormModel
            {
                Id = ticketPrice.Id,
                ScreeningType = ticketPrice.ScreeningType,
                TicketType = ticketPrice.TicketType,
                IsActive = ticketPrice.IsActive,
                Price = ticketPrice.Price
            };
        }
    }
}
