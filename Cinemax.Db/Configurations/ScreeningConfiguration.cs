using Cinemax.Domain.Entities;
using Cinemax.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemax.Db.Configurations
{
    public class ScreeningConfiguration : IEntityTypeConfiguration<Screening>
    {
        public void Configure(EntityTypeBuilder<Screening> builder)
        {
            builder.Property(x => x.Status)
                .HasConversion<string>();

            builder.Property(x => x.Status)
                .HasDefaultValue(ScreeningStatus.Scheduled);
        }
    }
}
