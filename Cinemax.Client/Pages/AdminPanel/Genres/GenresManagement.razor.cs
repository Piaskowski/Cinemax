using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Client.Services;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Genres;
using System.Net.Http;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Genres
{
    public partial class GenresManagement
    {
        private DataGrid<GenreDto>? _genresGrid;
        private ConfirmDialog? _deleteDialog;
        private readonly string _genresFormDialogId = "genres-edit-form";
        private readonly string _deleteGenreModalId = "confirm-delete-modal";
        private GenreFormModel _selectedGenre = new();
        private GenreDto? _genreToDelete;
        private bool _isDeleting = false;
        private async Task<GridResponse<GenreDto>> LoadData(GridRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/admin/Genres", request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<GenreDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<GenreDto>>() ??
                new GridResponse<GenreDto>();
        }

        private async Task RefreshDataAsync()
        {
            _selectedGenre = new();
            await _genresGrid!.ReloadAsync();
        }

        private void SelectGenre(GenreDto selectedGenre)
        {
            _selectedGenre = new GenreFormModel{
                Id = selectedGenre.Id,
                Name = selectedGenre.Name,
                IsActive = selectedGenre.IsActive
            };
        }

        private void SelectGenreForDelete(GenreDto genre)
        {
            _genreToDelete = genre;
        }

        private async Task DeleteGenreAsync()
        {
            if (_genreToDelete == null)
            {
                return;
            }

            _isDeleting = true;
            try
            {
                var response = await _httpClient.DeleteAndReadAsync(
                    $"api/admin/Genres/{_genreToDelete.Id}");

                if (!response.IsSuccess)
                {
                    _notificationService.Error(
                        response.ErrorMessage ?? "Nie udało się usunąć gatunku."
                    );

                    return;
                }

                _notificationService.Success("Gatunek został usunięty.");
                await _deleteDialog!.CloseAsync();
                _genreToDelete = null;
                await RefreshDataAsync();
            }
            finally
            {
                _isDeleting = false;
            }
        }
    }
}
