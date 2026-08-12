using Cinemax.Shared.Contracts.Movies;
using Microsoft.AspNetCore.Components;

namespace Cinemax.Client.Pages.Shared
{
    public partial class ScreeningShowtimes
    {
        [Parameter]
        public IEnumerable<MovieShowtimesDto> MovieShowtimes { get; set; } = [];
        [Parameter]
        public DateOnly SelectedDate { get; set; }
        [Parameter]
        public EventCallback<DateOnly> ChangeDate { get; set; }
        [Parameter]
        public bool ShowBreadCrumbs { get; set; } = true;

        private async Task OnDateChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var parsedDateTime))
            {
                await ChangeDate.InvokeAsync(DateOnly.FromDateTime(parsedDateTime));
            }
        }
    }
}
