using Cinemax.Application.Elements.Notifications.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Db.Repositories.Notifications
{
    public class EmailMessageRepository(AppDbContext dbContext) : BaseRepository<MessageTemplate, Guid>(dbContext), IEmailMessageRepository
    {
        public async Task<MessageTemplate?> GetByCode(string code)
        {
            return await _dbContext.Set<MessageTemplate>().
                FirstOrDefaultAsync(x => x.Code == code);
        }
    }
}
