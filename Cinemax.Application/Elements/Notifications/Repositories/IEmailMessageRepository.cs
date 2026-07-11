using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities.Notifications;

namespace Cinemax.Application.Elements.Notifications.Repositories
{
    public interface IEmailMessageRepository : IBaseRepository<MessageTemplate, Guid>
    {
    }
}
