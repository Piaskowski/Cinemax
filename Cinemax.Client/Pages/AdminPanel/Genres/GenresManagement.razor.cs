using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Genres;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Genres
{
    public partial class GenresManagement
    {
        private DataGrid<GenreDto>? _genresGrid;
        private readonly string _genresFormDialogId = "genres-edit-form";
        private GenreFormModel _selectedGenre = new();
        private async Task<GridResponse<GenreDto>> LoadData(GridRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync("api/admin/Genres", request);

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
    }
}
