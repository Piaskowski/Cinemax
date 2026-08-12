using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.TicketPrices;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.TicketPrices
{
    public partial class TicketPricesManagement
    {
        private DataGrid<TicketPriceDto>? _ticketPricesGrid;
        private ConfirmDialog? _deleteDialog;
        private TicketPriceFormModel _selectedTicketPrice = new();
        private TicketPriceDto? _ticketPriceToDelete;
        private readonly string _createTicketPriceDialogId = "ticket-price-form-modal";
        private readonly string _deleteTicketPriceModalId = "delete-ticket-price-modal";
        private bool _isDeleting = false;
        private async Task<GridResponse<TicketPriceDto>> LoadData(GridRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/admin/TicketPrices", request);

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

        private void SelectTicketPriceForDelete(TicketPriceDto ticketPrice)
        {
            _ticketPriceToDelete = ticketPrice;
        }

        private async Task DeleteTicketPriceAsync()
        {
            if (_ticketPriceToDelete == null)
            {
                return;
            }

            _isDeleting = true;
            try
            {
                var response = await _httpClient.DeleteAndReadAsync(
                    $"api/admin/TicketPrices/{_ticketPriceToDelete.Id}");

                if (!response.IsSuccess)
                {
                    _notificationService.Error(
                        response.ErrorMessage ?? "Nie udało się usunąć ceny."
                    );

                    return;
                }

                _notificationService.Success("Cena została usunięta.");
                await _deleteDialog!.CloseAsync();
                _ticketPriceToDelete = null;
                await RefreshDataAsync();
            }
            finally
            {
                _isDeleting = false;
            }
        }
    }
}
