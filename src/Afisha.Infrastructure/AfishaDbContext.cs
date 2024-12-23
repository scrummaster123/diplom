using Afisha.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure;

public class AfishaDbContext(DbContextOptions<AfishaDbContext> options) : DbContext(options)
{
    /// <summary>
    ///     Локации для проведения мероприятий
    /// </summary>
    public DbSet<Location> Locations => Set<Location>();

    /// <summary>
    ///     Пользователи системы
    /// </summary>
    public DbSet<User> Users => Set<User>();
}