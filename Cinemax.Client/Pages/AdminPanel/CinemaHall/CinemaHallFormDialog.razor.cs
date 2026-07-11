using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.CinemaHalls;
using Microsoft.AspNetCore.Components;

namespace Cinemax.Client.Pages.AdminPanel.CinemaHall
{
    public partial class CinemaHallFormDialog
    {
        [Parameter] public required string Id { get; set; }
        [Parameter] public CinemaHallFormModel Hall { get; set; } = new();
        [Parameter] public EventCallback OnSuccess { get; set; }

        private AdminFormDialog<CinemaHallFormModel>? _form;
        private bool IsEditMode => Hall.Id.HasValue;

        private string? _errorMessage;
        private IEnumerable<string> _errors = [];
        private bool _isSubmitting = false;

        private async Task OnValidSubmit()
        {
            if (IsEditMode) 
            {
                await HandleEditCinemaHall();
            }
            else
            {
                await HandleCreateCinemaHall();
            }
        }
        private async Task HandleCreateCinemaHall()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;

            var request = new CreateCinemaHallRequest
            {
                Number = Hall.Number,
                Type = Hall.Type
            };

            try
            {
                var response = await _httpClient.PostAndReadAsync("api/admin/CinemaHalls/create-cinemahall", 
                    request);

                if (response.IsSuccess)
                {
                    _notificationService.Success("Sala została utworzona pomyślnie");
                    Hall = new();
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

        private async Task HandleEditCinemaHall()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;

            var request = new EditCinemaHallRequest
            {
                Id = Hall.Id!.Value,
                Number = Hall.Number,
                Type = Hall.Type,
                IsActive = Hall.IsActive
            };

            try
            {
                var response = await _httpClient.PostAndReadAsync("api/admin/CinemaHalls/edit-cinemahall",
                    request);

                if (response.IsSuccess)
                {
                    Hall = new();
                    _notificationService.Success("Edycja zakończona pomyślnie.");
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
            Hall = new();
            _errorMessage = null;
            _errors = [];
        }
    }
}
