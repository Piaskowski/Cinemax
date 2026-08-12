using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.CinemaHalls;
using Cinemax.Shared.Contracts.Movies;
using Cinemax.Shared.Contracts.Scrennings;
using Cinemax.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace Cinemax.Client.Pages.AdminPanel.Screenings
{
    public partial class ScreeningFormDialog
    {
        [Parameter] public required string Id { get; set; }
        [Parameter] public ScreeningFormModel Screening { get; set; } = new();
        [Parameter] public EventCallback OnSuccess { get; set; }

        private AdminFormDialog<ScreeningFormModel>? _form;

        private IEnumerable<CinemaHallSelectItemDto> _halls = [];
        private IEnumerable<MovieSelectItemDto> _movies = [];
        private bool IsEditMode => Screening.Id.HasValue;
        private string Title => IsEditMode ? "Edycja seansu" : "Dodawanie seansu";
        private string? _errorMessage;
        private IEnumerable<string> _errors = [];
        private bool _isSubmitting = false;

        protected async override Task OnInitializedAsync()
        {
            _halls = await GetCinemaHallsList();
            _movies = await GetMoviesList();
            StateHasChanged();
        }

        private async Task OnValidSubmit()
        {
            if (IsEditMode)
            {
                await HandleEditScreening();
            }
            else
            {
                await HandleCreateScreening();
            }
        }

        public async Task HandleCreateScreening()
        {
            _isSubmitting = true;
            _errorMessage = null;
            _errors = [];

            var request = new CreateScreeningRequest
            {
                MovieId = Screening.MovieId,
                CinemaHallId = Screening.CinemaHallId,
                StartTime = Screening.StartTime
            };

            try
            {
                var response = await _httpClient.PostAndReadAsync(
                    "api/admin/Screenings/create-screening",
                    request);

                if (response.IsSuccess)
                {
                    Screening = new();
                    _notificationService.Success("Seans utworzony pomyślnie.");

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

        public async Task HandleEditScreening()
        {
            _isSubmitting = true;
            _errorMessage = null;
            _errors = [];

            var request = new EditScreeningRequest
            {
                Id = Screening.Id!.Value,
                MovieId = Screening.MovieId,
                CinemaHallId = Screening.CinemaHallId,
                StartTime = Screening.StartTime,
                Status = Screening.Status
            };

            try
            {
                var response = await _httpClient.PostAndReadAsync(
                    "api/admin/Screenings/edit-screening",
                    request);

                if (response.IsSuccess)
                {
                    Screening = new();
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
            Screening = new();
            _errorMessage = null;
            _errors = [];
        }
        private async Task<IEnumerable<CinemaHallSelectItemDto>> GetCinemaHallsList()
        {
            return await GetList<CinemaHallSelectItemDto>("api/admin/CinemaHalls/list");
        }

        private async Task<IEnumerable<MovieSelectItemDto>> GetMoviesList()
        {
            return await GetList<MovieSelectItemDto>("api/admin/Movies/list");
        }

        private async Task<IEnumerable<T>> GetList<T>(string url) where T : class
        {
            var response = await _httpClient.GetAndReadAsync<IEnumerable<T>>(url);

            if (response.IsSuccess)
            {
                return response.Data ?? [];
            }
            else
            {
                return [];
            }
        }

        private bool DisableStatusInputSelect()
        {
            return (Screening.Status.Equals(ScreeningStatus.Cancelled) || Screening.Status.Equals(ScreeningStatus.Finished));
        }
    }
}
