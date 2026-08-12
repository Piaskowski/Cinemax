using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Client.Services;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Scrennings;
using System.Net.Http;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Screenings
{
    public partial class ScreeningsManagement
    {
        private DataGrid<ScreeningDto>? _screeningGrid;
        private ConfirmDialog? _deleteDialog;
        private ScreeningFormModel _selectedScreening = new();
        private ScreeningDto? _screeningToDelete;
        private readonly string _createScreeningDialogId = "screening-form-modal";
        private readonly string _importScreeningsDialogId = "import-screenings-modal";
        private readonly string _deleteScreeningModalId = "delete-screening-modal";
        private bool _isDeleting = false;
        public async Task<GridResponse<ScreeningDto>> LoadData(GridRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/admin/Screenings", request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<ScreeningDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<ScreeningDto>>() ?? new GridResponse<ScreeningDto>();
        }

        public async Task RefreshDataAsync()
        {
            _selectedScreening = new();
            await _screeningGrid!.ReloadAsync();
        }

        private void SelectScreening(ScreeningDto screening)
        {
            _selectedScreening = new ScreeningFormModel
            {
                Id = screening.Id,
                MovieId = screening.Movie.Id,
                CinemaHallId = screening.CinemaHall.Id,
                StartTime = screening.StartTime,
                Status = screening.Status
            };
        }

        private void SelectScreeningForDelete(ScreeningDto screening)
        {
            _screeningToDelete = screening;
        }

        private async Task DeleteScreeningAsync()
        {
            if (_screeningToDelete == null)
            {
                return;
            }

            _isDeleting = true;
            try
            {
                var response = await _httpClient.DeleteAndReadAsync(
                    $"api/admin/Screenings/{_screeningToDelete.Id}");

                if (!response.IsSuccess)
                {
                    _notificationService.Error(
                        response.ErrorMessage ?? "Nie udało się usunąć seansu."
                    );

                    return;
                }

                _notificationService.Success("Seans został usunięty.");
                await _deleteDialog!.CloseAsync();
                _screeningToDelete = null;
                await RefreshDataAsync();
            }
            finally
            {
                _isDeleting = false;
            }
        }

        public async Task ImportFromFile()
        {

        }
    }
}
