using Cinemax.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemax.Db.Configurations
{
    public class EmailNotificationConfiguration : IEntityTypeConfiguration<EmailNotification>
    {
        public void Configure(EntityTypeBuilder<EmailNotification> builder)
        {
            builder.Property(x => x.Status)
                .HasConversion<string>();
        }
    }
}
