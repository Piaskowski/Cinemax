using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.Notifications.Repositories;
using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Elements.Reservations.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Elements.Seats.Repositories;
using Cinemax.Application.Elements.Tickets.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Constants;
using Cinemax.Domain.Entities.Notifications;
using Cinemax.Domain.Entities.Orders;
using Cinemax.Shared.Contracts.Orders;
using Cinemax.Shared.Enums;
using Cinemax.Shared.Resources.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Orders.Services
{
    public class OrderService(IScreeningRepository screenings,
        ITicketRepository tickets,
        ISeatRepository seats,
        IOrderRepository orders,
        IReservationRepository reservations,
        IPaymentService paymentService,
        ICurrentUserService currentUserService,
        IEmailNotificationRepository notifiations,
        IEmailMessageRepository emailTemplates,
        IEmailBodyRenderer bodyRenderer,
        IQrCodeService qrCodeService) : IOrderService
    {

        private readonly IOrderRepository _orders = orders;
        private readonly IScreeningRepository _screenings = screenings;
        private readonly ITicketRepository _tickets = tickets;
        private readonly ISeatRepository _seats = seats;
        private readonly IReservationRepository _reservations = reservations;
        private readonly IEmailNotificationRepository _notifications = notifiations;
        private readonly IEmailMessageRepository _emailTemplates = emailTemplates;

        private readonly IPaymentService _paymentService = paymentService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IEmailBodyRenderer _bodyRenderer = bodyRenderer;
        private readonly IQrCodeService _qrCodeService = qrCodeService;

        public async Task<Order> PrepareOrderAsync(NewOrderRequest request, CancellationToken ct)
        {
            if (!request.Reservations.Any())
                throw new ValidationException([ValidationMessages.Error_OrderContainsNoSeats]);

            // == check if reservations are valid ==
            var screening = await _screenings.Query()
                .FirstOrDefaultAsync(s => s.Id == request.ScreeningId, ct)
                ?? throw new NotFoundException(ValidationMessages.Screening_NotExists);

            var seatIds = request.Reservations
                .Select(r => r.SeatId)
                .ToList();

            if (seatIds.Distinct().Count() != seatIds.Count)
                throw new ValidationException([ValidationMessages.Error_OrderContainsDuplicateSeats]);

            var seats = await _seats.Query()
                .Where(s => seatIds.Contains(s.Id) &&
                    s.CinemaHallId == screening.CinemaHallId &&
                    s.IsActive)
                .ToListAsync(ct);

            if (seats.Count != request.Reservations.Count())
                throw new ValidationException([ValidationMessages.Error_OrderContainsInvalidSeats]);

            var isAnySeatsReserved = await _reservations.Query()
                .AnyAsync(r => r.Order.ScreeningId == request.ScreeningId &&
                    (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed) &&
                    seatIds.Contains(r.SeatId),
                    ct);

            if (isAnySeatsReserved)
                throw new ValidationException([ValidationMessages.Error_SelectedSeatsExpired]);

            var ticketPrices = await _tickets.Query()
                .Where(t => t.ScreeningType == screening.ScreeningType &&
                    t.IsActive)
                .ToListAsync(ct);

            var orderReservations = new List<Reservation>();
            foreach (var reservation in request.Reservations)
            {
                var seat = seats.First(s => s.Id == reservation.SeatId);
                var ticketPrice = ticketPrices.FirstOrDefault(t => t.TicketType == reservation.TicketType &&
                        t.SeatType == seat.Type);

                if (ticketPrice is null)
                    throw new ValidationException(
                        [ValidationMessages.Error_TicketTypeUnavailableForSeat]);

                orderReservations.Add(new Reservation
                {
                    SeatId = reservation.SeatId,
                    Seat = seat,
                    Status = ReservationStatus.Pending,
                    TicketType = reservation.TicketType,
                    FinalPrice = ticketPrice.Price
                });
            }

            return new Order
            {
                CustomerEmail = request.CustomerEmail,
                ScreeningId = request.ScreeningId,
                UserId = _currentUserService.Id,
                CreatedByUserId = _currentUserService.Id,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = screening.StartTime.AddHours(-1),
                Reservations = orderReservations
            };
        }

        public async Task MarkOrderAsConfirmed(Guid orderId, CancellationToken ct)
        {
            var order = await _orders.Query()
                .Include(o => o.Reservations)
                .FirstOrDefaultAsync(o => o.Id == orderId, ct)
                ?? throw new NotFoundException(ValidationMessages.NotFound_Order);

            if (order.Status == OrderStatus.Confirmed)
                return;

            if (order.Status != OrderStatus.Pending)
                throw new ValidationException(
                    [ValidationMessages.Error_OrderCannotBeConfirmedInCurrentState]);

            var pendingReservations = order.Reservations.Where(r => r.Status == ReservationStatus.Pending).ToList();

            if (pendingReservations.Count == 0)
                throw new ValidationException([ValidationMessages.Error_OrderContainsNoPendingReservations]);

            order.Status = OrderStatus.Confirmed;
            foreach (var item in pendingReservations)
            {
                item.Status = ReservationStatus.Confirmed;
            }

            await _orders.UpdateAsync(order, ct);

            if (!string.IsNullOrEmpty(order.CustomerEmail))
                await GenerateNotification(order, ct);
        }
        public async Task<Order?> GetUserOrder(Guid id, CancellationToken ct)
        {
            var userId = _currentUserService.Id;
            if (userId == null)
                return null;

            return await _orders.Query()
                .Include(o => o.Reservations)
                    .ThenInclude(r => r.Seat)
                .Include(o => o.Screening)
                    .ThenInclude(s => s.Movie)
                .Include(o => o.Screening)
                    .ThenInclude(s => s.CinemaHall)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId, ct);
        }

        public async Task<List<Order>> GetUserOrdersList(int page, int itemsPerPage, CancellationToken ct)
        {
            var userId = _currentUserService.Id;
            if (userId == null)
                return [];

            return await _orders.GetPaged(page, itemsPerPage)
                .Include(o => o.Screening)
                .ThenInclude(s => s.Movie)
                .Where(orders => orders.UserId == userId)
                .ToListAsync(ct);
        }

        public async Task<int> GetUserOrdersListTotalPages(int ordersPerPage, CancellationToken ct)
        {
            var userId = _currentUserService.Id;
            if (userId == null)
                return 0;

            return (int)Math.Ceiling((double)await _orders.Query()
                .Where(orders => orders.UserId == userId)
                .CountAsync(ct) / ordersPerPage);
        }

        private async Task GenerateNotification(Order order, CancellationToken ct)
        {
            var template = await _emailTemplates.GetByCode("Ticket");

            var values = new Dictionary<string, string>
            {
                ["OrderId"] = order.Id.ToString(),
                ["MovieTitle"] = order.Screening.Movie.Title,
                ["ScreeningDate"] = order.Screening.StartTime.ToString("dd.MM.yyyy"),
                ["ScreeningTime"] = order.Screening.StartTime.ToString("HH:mm"),
                ["CinemaHallNumber"] = order.Screening.CinemaHall.Number.ToString(),
                ["Tickets"] = BuildTicketsHtml(order.Reservations)
            };

            var qrCode = _qrCodeService.Generate(order.QrToken!);

            var notification = new EmailNotification
            {
                Subject = _bodyRenderer.Render(template!.Subject, values),
                Body = _bodyRenderer.Render(template.Body, values),
                To = order.CustomerEmail!,
                Status = NotificationStatus.Pending,
                Resources =
                    [
                        new EmailNotificationResource
                        {
                            ContentId = "ticket-qr",
                            FileName = "ticket-qr.png",
                            Content = qrCode!
                        }
                    ],
                MessageTemplateId = template.Id
            };

            await _notifications.CreateAsync(notification, ct);
        }

        private static string BuildTicketsHtml(IEnumerable<Reservation> reservations)
        {
            var groups = reservations
                .GroupBy(r => new
                {
                    r.TicketType,
                    r.Seat.Type
                });

            var rows = groups.Select(group => $"""
                <tr>
                    <td style="padding:8px 0;">
                        <strong>{group.Key.Type}</strong>
                        {group.Key.TicketType}
                    </td>
                    <td style="padding:8px 0; text-align:right;">
                        × {group.Count()}
                    </td>
                </tr>
                """);

                    return $"""
                <table width="100%" cellpadding="0" cellspacing="0">
                    {string.Join(Environment.NewLine, rows)}
                </table>
                """;
        }
    }
}
