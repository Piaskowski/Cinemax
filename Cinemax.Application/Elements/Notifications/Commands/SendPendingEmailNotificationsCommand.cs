using MediatR;

namespace Cinemax.Application.Elements.Notifications.Commands
{
    public record SendPendingEmailNotificationsCommand : IRequest;
}
