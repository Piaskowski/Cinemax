using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Identity;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Users
{
    public partial class UsersManagement
    {
        private readonly string _createUserDialogId = "create-user-modal";
        private DataGrid<UserDto>? _userGrid;
        private async Task<GridResponse<UserDto>> LoadData(GridRequest request)
        {
            var response = await HttpClient.PostAsJsonAsync(
                "api/admin/Users",
                request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<UserDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<UserDto>>()
                ?? new GridResponse<UserDto>();
        }

        private async Task RefreshDataAsync()
        {
            await _userGrid!.ReloadAsync();
        }
    }
}
