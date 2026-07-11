using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Domain.Entities.Orders;
using Cinemax.Shared.Contracts.TicketPrices;
using Microsoft.AspNetCore.Components;

namespace Cinemax.Client.Pages.AdminPanel.TicketPrices
{
    public partial class TicketPriceFormDialog
    {
        [Parameter]
        public required string Id { get; set; }
        [Parameter] public TicketPriceFormModel TicketPrice { get; set; } = new();
        [Parameter] public EventCallback OnSuccess { get; set; }

        private AdminFormDialog<TicketPriceFormModel>? _form;
        private bool IsEditMode => TicketPrice.Id.HasValue;

        private string? _errorMessage;
        private IEnumerable<string> _errors = [];
        private bool _isSubmitting = false;

        private async Task OnValidSubmit()
        {
            if (IsEditMode)
            {
                await HandleEditTicketPrice();
            }
            else
            {
                await HandleCreateTicketPrice();
            }
        }
        private async Task HandleCreateTicketPrice()
        {
            _isSubmitting = true;
            _errorMessage = null;
            _errors = [];

            var request = new CreateTicketPriceRequest
            {
                TicketType = TicketPrice.TicketType,
                ScreeningType = TicketPrice.ScreeningType,
                Price = TicketPrice.Price
            };

            try
            {
                var response = await HttpClient.PostAndReadAsync(
                    "api/admin/TicketPrices/create-ticket",
                    request);

                if (response.IsSuccess)
                {
                    NotificationService.Success("Cena biletu została utworzona pomyślnie.");

                    TicketPrice = new();
                    await _form!.CloseAsync();
                    await OnSuccess.InvokeAsync();
                }
                else
                {
                    _errorMessage = response.ErrorMessage;
                    _errors = response.Errors;
                }
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private async Task HandleEditTicketPrice()
        {
            _isSubmitting = true;
            _errorMessage = null;
            _errors = [];

            var request = new EditTicketPriceRequest
            {
                Id = TicketPrice.Id!.Value,
                TicketType = TicketPrice.TicketType,
                ScreeningType = TicketPrice.ScreeningType,
                Price = TicketPrice.Price,
                IsActive = TicketPrice.IsActive
            };

            try
            {
                var response = await HttpClient.PostAndReadAsync(
                    "api/admin/TicketPrices/edit-ticket",
                    request);

                if (response.IsSuccess)
                {
                    NotificationService.Success("Edycja zakończona pomyślnie.");

                    TicketPrice = new();
                    await _form!.CloseAsync();
                    await OnSuccess.InvokeAsync();
                }
                else
                {
                    _errorMessage = response.ErrorMessage;
                    _errors = response.Errors;
                }
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private void HandleCancel()
        {
            TicketPrice = new();
            _errorMessage = null;
            _errors = [];
        }
    }
}
