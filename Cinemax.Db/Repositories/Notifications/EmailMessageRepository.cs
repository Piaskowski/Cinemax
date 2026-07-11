using Cinemax.Application.Elements.Notifications.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities.Notifications;

namespace Cinemax.Db.Repositories.Notifications
{
    public class EmailMessageRepository(AppDbContext dbContext) : BaseRepository<MessageTemplate, Guid>(dbContext), IEmailMessageRepository
    {
    }
}
