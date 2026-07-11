using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Genres;
using Microsoft.AspNetCore.Components;

namespace Cinemax.Client.Pages.AdminPanel.Genres
{
    public partial class GenreFormDialog
    {
        [Parameter] public required string Id { get; set; }
        [Parameter] public GenreFormModel Genre { get; set; } = new();
        [Parameter] public EventCallback OnSuccess { get; set; } 

        private AdminFormDialog<GenreFormModel>? _form;
        private bool IsEditMode => Genre.Id.HasValue;

        private string? _errorMessage;
        private IEnumerable<string> _errors = [];
        private bool _isSubmitting = false;


        private async Task OnValidSubmit()
        {
            if (IsEditMode)
            {
                await HandleEditGenre();
            }
            else
            {
                await HandleCreateGenre();
            }
        }
        private async Task HandleCreateGenre()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;

            var request = new CreateGenreRequest
            {
                Name = Genre.Name
            };

            try
            {
                var response = await _httpClient.PostAndReadAsync("api/admin/Genres/create-genre", request);

                if (response.IsSuccess)
                {
                    Genre = new();
                    _notificationService.Success("Gatunek utworzony pomyślnie.");

                    await _form!.CloseAsync();
                    await OnSuccess.InvokeAsync();
                }
                else
                {
                    _errors = response.Errors;
                    _errorMessage = response.ErrorMessage;

                }
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private async Task HandleEditGenre()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;

            var request = new EditGenreRequest
            {
                Id = Genre.Id!.Value,
                Name = Genre.Name,
                IsActive = Genre.IsActive
            };

            try
            {
                var response = await _httpClient.PostAndReadAsync("api/admin/Genres/edit-genre", request);

                if (response.IsSuccess)
                {
                    Genre = new();
                    _notificationService.Success("Edycja zakończona pomyślnie.");

                    await _form!.CloseAsync();
                    await OnSuccess.InvokeAsync();
                }
                else
                {
                    _errors = response.Errors;
                    _errorMessage = response.ErrorMessage;

                }
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private void HandleCancel()
        {
            Genre = new();
            _errorMessage = null;
            _errors = [];
        }
    }
}
