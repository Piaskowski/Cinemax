using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Genres;
using Cinemax.Shared.Contracts.Movies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Movies
{
    public partial class MovieFormDialog
    {
        [Parameter] 
        public required string Id { get; set; }
        [Parameter] public MovieFormModel Movie { get; set; } = new();
        [Parameter] public EventCallback OnSuccess { get; set; }

        private AdminFormDialog<MovieFormModel>? _form;
        private IEnumerable<GenreDto> _genres = [];
        private IBrowserFile? _posterFile;
        private bool IsEditMode => Movie.Id.HasValue;
        private string Title => IsEditMode ? "Edycja filmu" : "Dodawanie filmu";
        private string? _posterValidationMessage = string.Empty;
        private string? _errorMessage;
        private IEnumerable<string> _errors = [];
        private int _maxFileSize = 2 * 1024 * 1024;
        private bool _isSubmitting = false;

        protected async override Task OnInitializedAsync()
        {
            _genres = await GetGenresList();
            StateHasChanged();
        }

        private async Task OnValidSubmit()
        {
            if (IsEditMode)
            {
                await HandleEditMovie();
            }
            else
            {
                await HandleCreateMovie();
            }
        }

        private async Task<IEnumerable<GenreDto>> GetGenresList()
        {
            var response = await HttpClient.GetAndReadAsync<GetGenresResponse>("api/admin/Genres");

            if (response.IsSuccess) {
                return response.Data!.Items;
            }
            else
            {
                return [];
            }
        }

        private async Task HandleCreateMovie()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;
            _posterValidationMessage = HandleFileValidation(_posterFile, _maxFileSize);

            if (!string.IsNullOrEmpty(_posterValidationMessage))
            {
                _isSubmitting = false;
                return;
            }

            var request = new CreateMovieRequest
            {
                Title = Movie.Title,
                Director = Movie.Director,
                Description = Movie.Description,
                DurationMinutes = Movie.DurationMinutes,
                TrailerUrl = Movie.TrailerUrl,
                GenresIds = Movie.GenresIds
            };

            try
            {
                request.PosterUrl = await UploadPosterAsync(_posterFile!);

                var response = await HttpClient.PostAndReadAsync("api/admin/Movies/create-movie", request);

                if (response.IsSuccess)
                {
                    NotificationService.Success("Film został utworzony pomyślnie.");
                    Movie = new();
                    _posterFile = null;
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

        private async Task HandleEditMovie()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;
            _posterValidationMessage = CheckFileSize(_posterFile, _maxFileSize);

            if (!string.IsNullOrEmpty(_posterValidationMessage))
            {
                _isSubmitting = false;
                return;
            }

            var request = new EditMovieRequest
            {
                Id = Movie.Id!.Value,
                Title = Movie.Title,
                Director = Movie.Director,
                Description = Movie.Description,
                DurationMinutes = Movie.DurationMinutes,
                TrailerUrl = Movie.TrailerUrl,
                GenresIds = Movie.GenresIds,
                IsActive = Movie.IsActive
            };

            try
            {
                if (_posterFile != null)
                {
                    request.PosterUrl = await UploadPosterAsync(_posterFile!);
                }
                else
                {
                    request.PosterUrl = Movie.PosterUrl;
                }

                var response = await HttpClient.PostAndReadAsync("api/admin/Movies/edit-movie", request);

                if (response.IsSuccess)
                {
                    NotificationService.Success("Edycja zakończona pomyślnie.");
                    Movie = new();
                    _posterFile = null;
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

        private async Task<string?> UploadPosterAsync(IBrowserFile file)
        {

            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(
                file.OpenReadStream(maxAllowedSize: _maxFileSize));

            fileContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.Name);

            var response = await HttpClient.PostAsync(
                "api/admin/Files/add-movie-poster",
                content);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<FileUploadResponse>();

            return result?.Url;
        }
        private void OnPosterSelected(InputFileChangeEventArgs e)
        {
            _posterFile = e.File;
            _posterValidationMessage = null;
        }

        private string? HandleFileValidation(IBrowserFile? file, int maxSize)
        {
            if (file is null)
            {
                return "Plakat do filmu jest wymagany.";
            }
            return CheckFileSize(file, maxSize);
        }

        private string? CheckFileSize(IBrowserFile? file, int maxSize)
        {
            if (file != null && file.Size > maxSize)
            {
                return $"Rozmiar plakatu nie może przekraczać {maxSize / 1024 / 1024} Mb.";
            }

            return null;
        }
        private void HandleCancel()
        {
            Movie = new();
            _posterFile = null;
            _errorMessage = null;
            _posterValidationMessage = null;
            _errors = [];
        }
    }
}
