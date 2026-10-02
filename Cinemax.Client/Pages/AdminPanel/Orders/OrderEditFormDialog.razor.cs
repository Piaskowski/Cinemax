using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Orders;
using Cinemax.Shared.Contracts.Reservations;
using Cinemax.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace Cinemax.Client.Pages.AdminPanel.Orders
{
    public partial class OrderEditFormDialog
    {
        [Parameter] public required string Id { get; set; }
        [Parameter] public OrderEditFormModel OrderModel { get; set; } = new();
        [Parameter] public EventCallback OnSuccess { get; set; }

        private AdminFormDialog<OrderEditFormModel>? _form;
        private string _title = "Edycja zamówienia";
        private string? _errorMessage;
        private IEnumerable<string> _errors = [];
        private bool _isSubmitting = false;

        private async Task OnValidSubmit()
        {
            _isSubmitting = true;
            _errorMessage = null;
            _errors = [];

            var request = new EditOrderRequest
            {   
                Id = OrderModel.Id,
                Status = OrderModel.Status,
                Reservations = [.. OrderModel.Reservations.Select(r => new EditOrderReservationRequest
                {
                    Id = r.Id,
                    Status = r.Status
                })]
            };

            try
            {
                var response = await _httpClient.PostAndReadAsync(
                    "api/admin/orders/edit-order",
                    request);

                if (response.IsSuccess)
                {
                    OrderModel = new();
                    _notificationService.Success("Zamówienie zostało zaktualizowane pomyślnie!");

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
            OrderModel = new();
            _errorMessage = null;
            _errors = [];
        }
    }
}
