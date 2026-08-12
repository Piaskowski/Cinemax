using Cinemax.Client.Pages.Shared.AdminPanel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.CinemaHall
{
    public partial class CreateSeatsFormDialog
    {
        [Parameter] public required string Id { get; set; }
        [Parameter] public Guid? CinemaHallId { get; set; }
        [Parameter] public EventCallback OnCreated { get; set; }

        private AdminFormDialog<CreateSeatsRequest>? _form;
        private CreateSeatsRequest _seats = new();
        private readonly int _maxFileSize = 2 * 1024 * 1024;

        private string? _errorMessage;
        private IEnumerable<string> _errors = [];
        private bool _isSubmitting = false;

        private class CreateSeatsRequest
        {
            [Required(ErrorMessage = "Wybierz plik.")]
            public IBrowserFile? File { get; set; }
        }

        private async Task HandleCreateSeats()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;
            var fileValidationError = HandleFileValidation(_seats.File, _maxFileSize);

            if (!string.IsNullOrEmpty(fileValidationError))
            {
                _isSubmitting = false;
                return;
            }

            if (CinemaHallId is null)
            {
                _errorMessage = "Nie wybrano sali kinowej.";
                _isSubmitting = false;
                return;
            }

            try
            {
                using var content = new MultipartFormDataContent();

                await using var fileStream = _seats.File!.OpenReadStream(_maxFileSize);

                using var fileContent = new StreamContent(fileStream);

                fileContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(_seats.File.ContentType);

                content.Add(fileContent, "file", _seats.File.Name);

                var response = await _httpClient.PostAsync(
                    $"api/admin/CinemaHalls/{CinemaHallId}/create-seats",
                    content);

                if (response.IsSuccessStatusCode)
                {
                    _notificationService.Success("Miejsca zaimportowane pomyślnie.");
                    _seats = new();
                    await _form!.CloseAsync();
                    await OnCreated.InvokeAsync();

                    return;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errors = await response.Content
                        .ReadFromJsonAsync<List<string>>();

                    if (errors is not null && errors.Count > 0)
                    {
                        _errors = errors;
                    }
                    else
                    {
                        _errorMessage = await response.Content.ReadAsStringAsync();
                    }

                    return;
                }

                _errorMessage = "Nie udało się zaimportować miejsc.";
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private void OnFileSelected(InputFileChangeEventArgs e)
        {
            _seats.File = e.File;
        }

        private void HandleCanel() 
        {
            _seats = new();
            _errorMessage = string.Empty;
            _errors = [];
        }


        private string? HandleFileValidation(IBrowserFile? file, int maxSize)
        {
            if (file is null)
            {
                return "plik jest wymagany.";
            }
            if (file.Size > maxSize)
            {
                return $"Rozmiar pliku nie może przekraczać {maxSize / 1024 / 1024} Mb.";
            }

            return null;
        }
    }
}
