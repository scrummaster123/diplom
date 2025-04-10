using Afisha.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder
            .HasOne(e => e.Location)
            .WithMany(location => location.Events)
            .HasForeignKey(e => e.LocationId)
            .HasPrincipalKey(location => location.Id)
            .IsRequired();
    }
}

