using Cinemax.Client.Common.Http;
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
        private ConfirmDialog? _deleteDialog;
        private CinemaHallDto? _cinemaHallToDelete;
        private bool _isDeleting = false;
        private readonly string _createCinemahallModalId = "create-cinemahall-modal";
        private readonly string _createSeatsModalId = "create-seats-modal";
        private readonly string _deleteCinemahallModalId = "confirm-delete-modal";

        private async Task<GridResponse<CinemaHallDto>> LoadData(GridRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/admin/CinemaHalls", request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<CinemaHallDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<CinemaHallDto>>() ?? new GridResponse<CinemaHallDto>();
        }

        private async Task DeleteCinemaHallAsync()
        {
            if (_cinemaHallToDelete == null)
            {
                return;
            }

            _isDeleting = true;
            try
            {
                var response = await _httpClient.DeleteAndReadAsync(
                    $"api/admin/CinemaHalls/{_cinemaHallToDelete.Id}");

                if (!response.IsSuccess)
                {
                    _notificationService.Error(
                        response.ErrorMessage ?? "Nie udało się usunąć sali kinowej."
                    );

                    return;
                }

                _notificationService.Success("Sala kinowa została usunięta.");
                await _deleteDialog!.CloseAsync();
                _cinemaHallToDelete = null;
                await RefreshDataAsync();
            }
            finally
            {
                _isDeleting = false;
            }
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

        private void SelectCinemaHallForDelete(CinemaHallDto cinemaHall)
        {
            _cinemaHallToDelete = cinemaHall;
        }
    }
}
