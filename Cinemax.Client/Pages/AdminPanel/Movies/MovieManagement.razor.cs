using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.CinemaHalls;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Movies;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Movies
{
    public partial class MovieManagement
    {
        private DataGrid<MovieDto>? _moviesGrid;
        private ConfirmDialog? _deleteDialog;
        private string _createMovieDialogId = "create-movie-modal";
        private readonly string _deleteMovieModalId = "delete-movie-modal";
        private MovieFormModel _selectedMovie = new();
        private MovieDto? _movieToDelete;
        private bool _isDeleting = false;
        private async Task<GridResponse<MovieDto>> LoadData(GridRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/admin/Movies", request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<MovieDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<MovieDto>>() ?? new GridResponse<MovieDto>();
        }

        private void SelectMovie(MovieDto movie)
        {
            _selectedMovie = new MovieFormModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Director = movie.Director,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                PosterUrl = movie.PosterUrl,
                TrailerUrl = movie.TrailerUrl,
                GenresIds = [.. movie.Genres.Select(x => x.Id)],
                IsActive = movie.IsActive
            };
        }

        private void SelectMovieForDelete(MovieDto movie)
        {
            _movieToDelete = movie;
        }

        private async Task DeleteMovieAsync()
        {
            if (_movieToDelete == null)
            {
                return;
            }

            _isDeleting = true;
            try
            {
                var response = await _httpClient.DeleteAndReadAsync(
                    $"api/admin/Movies/{_movieToDelete.Id}");

                if (!response.IsSuccess)
                {
                    _notificationService.Error(
                        response.ErrorMessage ?? "Nie udało się usunąć Filmu."
                    );

                    return;
                }

                _notificationService.Success("Film został usunięty.");
                await _deleteDialog!.CloseAsync();
                _movieToDelete = null;
                await RefreshDataAsync();
            }
            finally
            {
                _isDeleting = false;
            }
        }

        private async Task RefreshDataAsync()
        {
            _selectedMovie = new();
            await _moviesGrid!.ReloadAsync();
        }
        private async Task ImportFromFile()
        {

        }
    }
}
