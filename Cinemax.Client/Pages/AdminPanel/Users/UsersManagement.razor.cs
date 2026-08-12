using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Identity;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Cinemax.Client.Pages.AdminPanel.Users
{
    public partial class UsersManagement
    {
        private UserFormModel _selectedUser  = new();
        private DataGrid<UserDto>? _userGrid;
        private ConfirmDialog? _deleteDialog;
        private readonly string _createUserDialogId = "create-user-modal";
        private readonly string _deleteUserModalId = "delete-user-modal";
        private UserDto? _userToDelete;
        private bool _isDeleting = false;


        private async Task<GridResponse<UserDto>> LoadData(GridRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/admin/Users",
                request);

            if (!response.IsSuccessStatusCode)
                return new GridResponse<UserDto>();

            return await response.Content.ReadFromJsonAsync<GridResponse<UserDto>>()
                ?? new GridResponse<UserDto>();
        }
        private void SelectUserForDelete(UserDto user)
        {
            _userToDelete = user;
        }

        private void SelectUser(UserDto user)
        {
            _selectedUser = new UserFormModel{ 
                   Id = user.Id,
                   FirstName = user.FirstName,
                   LastName = user.LastName,
                   Email = user.Email,
                   IsActive = user.IsActive
            };
        }

        private async Task DeleteUserAsync()
        {
            if (_userToDelete == null)
            {
                return;
            }

            _isDeleting = true;
            try
            {
                var response = await _httpClient.DeleteAndReadAsync(
                    $"api/admin/Users/{_userToDelete.Id}");

                if (!response.IsSuccess)
                {
                    _notificationService.Error(
                        response.ErrorMessage ?? "Nie udało się usunąć użytkownika."
                    );

                    return;
                }

                _notificationService.Success("Użytkownik został usunięty.");
                await _deleteDialog!.CloseAsync();
                _userToDelete = null;
                await RefreshDataAsync();
            }
            finally
            {
                _isDeleting = false;
            }
        }

        private async Task RefreshDataAsync()
        {
            await _userGrid!.ReloadAsync();
        }
    }
}
