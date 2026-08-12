using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Identity;
using Microsoft.AspNetCore.Components;
using System;

namespace Cinemax.Client.Pages.AdminPanel.Users
{
    public partial class UserFormDialog
    {
        [Parameter] public required string Id { get; set; }
        [Parameter] public UserFormModel User { get; set; } = new();
        [Parameter] public EventCallback OnCreated { get; set; }
        private bool IsEditMode => User.Id.HasValue;
        private string Title => IsEditMode ? "Edycja użytkownika" : "Dodawanie użytkownika";
        private AdminFormDialog<UserFormModel>? _form;


        private string? _errorMessage { get; set; }
        private IEnumerable<string> _errors { get; set; } = [];
        private bool _isSubmitting = false;

        private async Task HandleValidSubmit()
        {
            if (IsEditMode)
            {
                await EditUserHandler();
            }
            else
            {
                await CreateUserHandler();
            }
        }

        private async Task CreateUserHandler()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;

            var request = new CreateUserRequest
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                Role = User.Role,
            };

            try
            {
                var result = await _httpClient.PostAndReadAsync<CreateUserRequest, CreateUserResponse>(
                    "api/admin/Users/create-user",
                    request
                );

                if (!result.IsSuccess)
                {
                    _errorMessage = result.ErrorMessage;
                    _errors = result.Errors;
                    return;
                }

                _notificationService.Success(result.Data!.Message!);

                User = new();
                await _form!.CloseAsync();
                await OnCreated.InvokeAsync();
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private async Task EditUserHandler()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;

            var request = new EditUserRequest
            {
                Id = User.Id!.Value,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                Role = User.Role,
                IsActive = User.IsActive,
            };

            try
            {
                var result = await _httpClient.PostAndReadAsync<EditUserRequest>(
                    "api/admin/Users/edit-user",
                    request
                );

                if (!result.IsSuccess)
                {
                    _errorMessage = result.ErrorMessage;
                    _errors = result.Errors;
                    return;
                }

                _notificationService.Success("Edycja użytkownika zakończona pomyślnie.");

                User = new();
                await _form!.CloseAsync();
                await OnCreated.InvokeAsync();
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private void Cancelhandler()
        {
            User = new();
            _errorMessage = null;
            _errors = [];
        }
    }
}
