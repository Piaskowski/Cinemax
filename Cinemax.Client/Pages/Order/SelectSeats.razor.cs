using Cinemax.Client.Common.Http;
using Cinemax.Shared.Contracts.Scrennings;
using Cinemax.Shared.Contracts.Seats;
using Cinemax.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace Cinemax.Client.Pages.Order
{
    public partial class SelectSeats
    {
        [Parameter]
        public Guid Id { get; set; }

        private GetScreeningSeatsResponse _screening = new();
        //private HashSet<Guid> _selectedSeatsIds = new();
        private List<ScreeningSeatDto> _selectedSeats = [];

        private OrderStage _stage = OrderStage.SelectingSeats;
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            await GetSeats();
        }

        private async Task GetSeats()
        {
            _isLoading = true;
            try
            {
                var response = await _httpClient.GetAndReadAsync<GetScreeningSeatsResponse>($"api/showtimes/{Id.ToString()}");

                if (response.IsSuccess)
                {
                    _screening = response.Data!;
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void SelectSeat(ScreeningSeatDto seat)
        {
            if (seat.Status != ScreeningSeatStatus.Available)
                return;

            if (_selectedSeats.Contains(seat)) {
                _selectedSeats.Remove(seat);
                return;
            }

            _selectedSeats.Add(seat);
        }

        private void ProceedToTicketSelection()
        {
            _stage = OrderStage.EditingTickets;
        }

        private string GetSeatStatusClass(ScreeningSeatDto seat)
        {
            if (_selectedSeats.Contains(seat))
                return "selected";

            return seat.Status switch
            {
                ScreeningSeatStatus.Booked => "booked",
                ScreeningSeatStatus.Sold => "sold",
                _ => "available"
            };
        }

        private enum OrderStage
        {
            SelectingSeats,
            EditingTickets,
            PlacingOrder
        }
    }
}
