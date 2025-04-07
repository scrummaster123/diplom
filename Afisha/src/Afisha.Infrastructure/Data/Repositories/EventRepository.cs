using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Data.Repositories;

public class EventRepository (AfishaDbContext context) : IEventRepository
{
    public Task<List<Event>> GetEventsByFiltersAsync(DateOnly dateStart, DateOnly dateEnd, CancellationToken cancellationToken,
        long? locationId = null, long? sponsorId = null)
    {
        // Базовое получение событий с диапазоном дат
        var events = context.Events
            .Where(x => x.DateStart >= dateStart 
                        && x.DateStart <= dateEnd)
            .AsNoTracking();
        
        // Подключение опциональных фильтров
        if (locationId != null)
            events = events.Where(e => e.LocationId == locationId);
        if (sponsorId != null)
            events = events.Where(e => e.SponsorId == sponsorId);
        
        return events.ToListAsync(cancellationToken);
    }
}