using Cinemax.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemax.Db.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(order => order.Status)
                .HasConversion<string>();

            builder.Property(order => order.Source)
                .HasConversion<string>();

            builder.HasIndex(o => o.QrToken)
                .IsUnique()
                .HasFilter("[QrToken] IS NOT NULL");
        }
    }
}
