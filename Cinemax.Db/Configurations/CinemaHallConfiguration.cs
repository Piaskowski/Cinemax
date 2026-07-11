using Cinemax.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemax.Db.Configurations
{
    public class CinemaHallConfiguration : IEntityTypeConfiguration<CinemaHall>
    {
        public void Configure(EntityTypeBuilder<CinemaHall> builder)
        {
            builder.HasIndex(x => x.Number)
                .IsUnique();

            builder.Property(x => x.Type)
                .HasConversion<string>();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
        }
    }
}
