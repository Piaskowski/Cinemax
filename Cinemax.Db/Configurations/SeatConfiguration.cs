using Cinemax.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemax.Db.Configurations
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.Property(x => x.Type)
                .HasConversion<string>();

            builder.HasIndex(x => new { x.CinemaHallId, x.Number, x.Row })
                .IsUnique();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
        }
    }
}
