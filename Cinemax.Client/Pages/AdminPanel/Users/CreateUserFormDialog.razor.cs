using Cinemax.Client.Common.Http;
using Cinemax.Client.Pages.Shared.AdminPanel;
using Cinemax.Shared.Contracts.Identity;
using Microsoft.AspNetCore.Components;

namespace Cinemax.Client.Pages.AdminPanel.Users
{
    public partial class CreateUserFormDialog
    {
        [Parameter] public required string Id { get; set; }
        [Parameter] public EventCallback OnCreated { get; set; }

        private AdminFormDialog<CreateUserRequest>? _form;
        private CreateUserRequest _user = new();

        private string? _errorMessage { get; set; }
        private IEnumerable<string> _errors { get; set; } = [];
        private bool _isSubmitting = false;

        private async Task CreateUserHandler()
        {
            _errorMessage = null;
            _errors = [];
            _isSubmitting = true;

            try
            {
                var result = await HttpClient.PostAndReadAsync<CreateUserRequest, CreateUserResponse>(
                    "api/admin/Users/create-user",
                    _user
                );

                if (!result.IsSuccess)
                {
                    _errorMessage = result.ErrorMessage;
                    _errors = result.Errors;
                    return;
                }

                NotificationService.Success(result.Data!.Message!);

                _user = new();
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
            _user = new();
        }
    }
}
