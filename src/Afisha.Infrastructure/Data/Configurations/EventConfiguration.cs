using Afisha.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afisha.Infrastructure.Data.Configurations;
public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder
            .HasOne<User>()
            .WithMany(u => u.Events)
            .HasForeignKey(e => e.SponsorId)
            .IsRequired();
    }
}
