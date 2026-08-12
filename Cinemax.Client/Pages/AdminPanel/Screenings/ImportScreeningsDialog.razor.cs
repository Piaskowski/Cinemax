using Cinemax.Client.Pages.Shared.AdminPanel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Screenings
{
    public partial class ImportScreeningsDialog
    {
        [Parameter] public required string Id { get; set; }
        [Parameter] public EventCallback OnCreated { get; set; }

        private AdminFormDialog<ImportScreeningsRequest>? _form;
        private ImportScreeningsRequest _screenings = new();
        private readonly int _maxFileSize = 2 * 1024 * 1024;

        private string? _errorMessage;
        private IEnumerable<string> _errors = [];
        private bool _isSubmitting = false;

        private class ImportScreeningsRequest
        {
            [Required(ErrorMessage = "Wybierz plik.")]
            public IBrowserFile? File { get; set; }
        }

        private async Task HandleImportScreenings()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;

            var fileValidationError = HandleFileValidation(_screenings.File, _maxFileSize);

            if (!string.IsNullOrEmpty(fileValidationError))
            {
                _isSubmitting = false;
                return;
            }

            try
            {
                using var content = new MultipartFormDataContent();

                await using var fileStream = _screenings.File!.OpenReadStream(_maxFileSize);

                using var fileContent = new StreamContent(fileStream);

                fileContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(_screenings.File.ContentType);

                content.Add(fileContent, "file", _screenings.File.Name);

                var response = await _httpClient.PostAsync(
                    $"api/admin/Screenings/create-screenings",
                    content);

                if (response.IsSuccessStatusCode)
                {
                    _notificationService.Success("Seanse zaimportowane pomyślnie.");
                    _screenings = new();
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
            _screenings.File = e.File;
        }

        private void HandleCanel()
        {
            _screenings = new();
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
