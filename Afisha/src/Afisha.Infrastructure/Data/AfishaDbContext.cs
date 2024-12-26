using System;
using Afisha.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Data;

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

    /// <summary>
    /// Зарегистрированные мероприятия
    /// </summary>
    public DbSet<Event> Events => Set<Event>();

    /// <summary>
    /// Связь многие ко многим Event и Location
    /// </summary>
    public DbSet<LocationEvent> LocationEvents => Set<LocationEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AfishaDbContext).Assembly);
    }
}