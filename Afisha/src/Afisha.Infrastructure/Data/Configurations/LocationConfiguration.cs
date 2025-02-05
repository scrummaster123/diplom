using Afisha.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Data.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder
            .HasOne(location => location.Owner)
            .WithMany(user => user.Locations)
            .HasForeignKey(location => location.OwnerId)
            .HasPrincipalKey(user => user.Id)
            .IsRequired();
    }
}
