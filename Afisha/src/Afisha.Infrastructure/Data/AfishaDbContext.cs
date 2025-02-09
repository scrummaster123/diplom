using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces;
using Afisha.Infrastructure.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Data;

public class AfishaDbContext(DbContextOptions<AfishaDbContext> options) : DbContext(options), IUnitOfWork
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

    public DbSet<Rating> Ratings => Set<Rating>();

    /// <summary>
    /// UnitOfWork реализация
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><see cref="{int}"/>Количество строк базы данных, на которых повлияло выполненное изменение</returns>
    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AfishaDbContext).Assembly);
        modelBuilder.Seed();
    }
}