using Cinemax.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemax.Db.Configurations
{
    public class TicketPriceConfiguration : IEntityTypeConfiguration<TicketPrice>
    {
        public void Configure(EntityTypeBuilder<TicketPrice> builder)
        {
            builder.Property(x => x.ScreeningType)
                .HasConversion<string>();

            builder.Property(x => x.TicketType)
                .HasConversion<string>();

            builder.Property(x => x.Price)
                .HasPrecision(18, 2);

            builder.HasIndex(x => new { x.ScreeningType, x.TicketType })
                .IsUnique();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
        }
    }
}
