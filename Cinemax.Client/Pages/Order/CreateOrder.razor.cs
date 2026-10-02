using Cinemax.Client.Auth;
using Cinemax.Client.Common.Http;
using Cinemax.Shared.Contracts.Orders;
using Cinemax.Shared.Contracts.Reservations;
using Cinemax.Shared.Contracts.Scrennings;
using Cinemax.Shared.Contracts.Seats;
using Cinemax.Shared.Contracts.TicketPrices;
using Cinemax.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace Cinemax.Client.Pages.Order
{
    public partial class CreateOrder
    {
        [Parameter]
        public Guid Id { get; set; }

        private GetScreeningSeatsResponse _screening = new();
        private List<ScreeningSeatDto> _selectedSeats = [];
        private List<Ticket> _tickets = [];
        private List<CreateReservationRequest> _reservations = [];
        private OrderModel _order = new();
        private OrderSummaryDto _orderSummary = new();
        private IEnumerable<SelectTicketPriceDto> _ticketPrices = [];
        private Guid? _orderId;
        private DateTime? _expiresAt;


        private OrderStage _stage = OrderStage.SelectingSeats;
        private OrderAction _action;
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            await GetSeats();
            await GetTicketPrices();
            await GetUserOrderFormData();
        }

        private async Task OnValidSubmit(OrderAction action)
        {
            _action = action;
            if (action == OrderAction.Purchase)
            {
                await PurchaseTickets();
            }
            else
            {
                await ReserveSeats();
            }
        }

        private async Task ReserveSeats()
        {
            if (_tickets.Count == 0) 
                return;

            _isLoading = true;

            try
            {
                var request = new NewOrderRequest
                {
                    ScreeningId = Id,
                    CustomerEmail = _order.Email,
                    Reservations = _tickets.Select(t => new CreateReservationRequest
                    {
                        TicketType = t.TicketType,
                        SeatId = t.Seat.Id,
                    })
                };
                var response = await _httpClient.PostAndReadAsync<NewOrderRequest, ReserveSeatsResponse>("api/order/reserve-seats", request);

                if (response.IsSuccess)
                {
                    _orderId = response.Data!.OrderId;
                    _expiresAt = response.Data!.ExpiresAt;
                    _stage = OrderStage.OrderCreated;
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task PurchaseTickets()
        {
            if (_tickets.Count == 0)
                return;

            _isLoading = true;

            try
            {
                var request = new NewOrderRequest
                {
                    ScreeningId = Id,
                    CustomerEmail = _order.Email,
                    Reservations = _tickets.Select(t => new CreateReservationRequest
                    {
                        TicketType = t.TicketType,
                        SeatId = t.Seat.Id,
                    })
                };
                var response = await _httpClient.PostAndReadAsync<NewOrderRequest, PurchaseTicketsResponse>("api/order/purchase-tickets", request);

                if (response.IsSuccess)
                {
                    _navigationManager.NavigateTo(response.Data!.CheckoutUrl, forceLoad: true);
                }
            }
            finally 
            { 
                _isLoading = false; 
            }
        }

        private async Task GetSeats()
        {
            _isLoading = true;
            try
            {
                var response = await _httpClient.GetAndReadAsync<GetScreeningSeatsResponse>($"api/order/{Id}");

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

        private async Task GetTicketPrices()
        {
            _isLoading = true;
            try
            {
                var response = await _httpClient.GetAndReadAsync<IEnumerable<SelectTicketPriceDto>>($"api/order/get-ticket-prices?screeningId={Id}");

                if (response.IsSuccess)
                {
                    _ticketPrices = response.Data!;
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task GetUserOrderFormData()
        {
            var authState = await ((CustomAuthStateProvider)_authStateProvider).GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity!.IsAuthenticated)
            {
                var response = await _httpClient.GetAndReadAsync<GetUserOrderFormDataResponse>("api/order/get-form-data");
                if (response.IsSuccess) {
                    _order.Email = response.Data!.Email;
                }
            }
        }

        private async Task GetOrderSummary()
        {
            _isLoading = true;
            try
            {
                var request = new NewOrderRequest
                {
                    ScreeningId = Id,
                    CustomerEmail = _order.Email,
                    Reservations = _tickets.Select(t => new CreateReservationRequest { 
                        TicketType = t.TicketType,
                        SeatId = t.Seat.Id,
                    })
                };

                var response = await _httpClient.PostAndReadAsync<NewOrderRequest, OrderSummaryDto>($"api/order/get-order-details", request);

                if (response.IsSuccess)
                {
                    _orderSummary = response.Data!;
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void ProceedToTicketSelection()
        {
            _stage = OrderStage.EditingTickets;

            _tickets.Clear();
            _orderSummary = new();

            foreach (var seat in _selectedSeats)
            {
                _tickets.Add(new Ticket
                {
                    Seat = seat,
                    TicketType = TicketType.Normal,
                    Price = _ticketPrices
                        .Where(t => t.TicketType == TicketType.Normal && t.SeatType == seat.Type)
                        .Select(t => t.Price)
                        .First()
                });
            }
        }

        private void BackToTicketSelection()
        {
            _stage = OrderStage.EditingTickets;
            _orderSummary = new();
        }

        private async Task ProceedToOrderSummary()
        {
            _stage = OrderStage.OrderSummary;

            await GetOrderSummary();
        }

        private void GoToSeatsSelection()
        {
            _stage = OrderStage.SelectingSeats;
            _tickets.Clear();
            _reservations.Clear();
        }

        private enum OrderStage
        {
            SelectingSeats,
            EditingTickets,
            OrderSummary,
            OrderCreated
        }
    }
}
