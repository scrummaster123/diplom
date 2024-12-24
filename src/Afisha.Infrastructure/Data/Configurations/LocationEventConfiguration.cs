using Afisha.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afisha.Infrastructure.Data.Configurations;
public class LocationEventConfiguration : IEntityTypeConfiguration<LocationEvent>
{
    public void Configure(EntityTypeBuilder<LocationEvent> builder)
    {
        builder
            .HasOne(le => le.Location)
            .WithMany(l => l.LocationEvents)
            .HasForeignKey(le => le.LocationId)
            .IsRequired();

        builder
            .HasOne(le => le.Event)
            .WithMany(e => e.LocationEvents)
            .HasForeignKey(le => le.EventId)
            .IsRequired();

        builder
            .HasIndex(le => new { le.LocationId, le.EventId })
            .IsUnique();
    }
}
