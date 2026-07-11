using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.CinemaHalls;
using Cinemax.Shared.Contracts.Common;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.CinemaHall
{
    public partial class CinemaHallsManagement
    {
        private DataGrid<CinemaHallDto>? _cinemahallGrid;
        private CinemaHallFormModel _selectedCinemaHall = new();
        private readonly string _createCinemahallModalId = "create-cinemahall-modal";
        private readonly string _createSeatsModalId = "create-seats-modal";
        private async Task<GridResponse<CinemaHallDto>> LoadData(GridRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync("api/admin/CinemaHalls", request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<CinemaHallDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<CinemaHallDto>>() ?? new GridResponse<CinemaHallDto>();
        }
        
        private async Task RefreshDataAsync()
        {
            _selectedCinemaHall = new();
            await _cinemahallGrid!.ReloadAsync();
        }

        private void SelectCinemaHall(CinemaHallDto cinemaHall)
        {
            _selectedCinemaHall = new CinemaHallFormModel { 
                Id = cinemaHall.Id,
                Number = cinemaHall.Number,
                Type = cinemaHall.Type,
                IsActive = cinemaHall.IsActive,
            };
        }
    }
}
