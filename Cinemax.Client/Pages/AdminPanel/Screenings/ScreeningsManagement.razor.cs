using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Scrennings;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Screenings
{
    public partial class ScreeningsManagement
    {
        private DataGrid<ScreeningDto>? _screeningGrid;
        private ScreeningFormModel _selectedScreening = new();
        private readonly string _createScreeningDialogId = "screening-form-modal";
        public async Task<GridResponse<ScreeningDto>> LoadData(GridRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync("api/admin/Screenings", request);

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

        public async Task ImportFromFile()
        {

        }
    }
}
