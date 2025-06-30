using Afisha.Domain.Entities;
using Afisha.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Data.Seeding;

internal static class SeederExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var user1 = new User { Id = 1, FirstName = "Ivan", LastName = "Ivanov", Email = "t@t.com", Login = "Ivan", PasswordHash = "1" };
        var user2 = new User { Id = 2, FirstName = "Petr", LastName = "Petrovich", Email = "Petrovich@t.com", Login = "Petrovich", PasswordHash = "1" };

        modelBuilder.Entity<User>().HasData(
            user1, 
            user2
        );

        var location1 = new Location { Id = 1, Name = "ДК Колхозник", OwnerId=1, Pricing = 100 };
        var location2 = new Location { Id = 2, Name = "Supr puper mega place", OwnerId = 1, Pricing = 1000 };

        modelBuilder.Entity<Location>().HasData(
            location1,
            location2
        );

        modelBuilder.Entity<Event>().HasData(
            new Event { Id = 1, LocationId = 1, DateStart = DateOnly.FromDateTime(DateTime.Now) },
            new Event { Id = 2, LocationId = 1, DateStart = DateOnly.FromDateTime(DateTime.Now) }
        );

        modelBuilder.Entity<EventUser>().HasData(
            new EventUser{Id = 1, UserId = 1, UserRole = EventRole.Organizer, EventId = 1},
            new EventUser{Id = 2, UserId = 2, UserRole = EventRole.Organizer, EventId = 2}
        );
    }
}
