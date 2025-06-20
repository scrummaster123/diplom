using Afisha.Domain.Entities;
using Afisha.Domain.Enums;
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
            .Include(x => x.EventParticipants)
            .Include(x => x.Location)
            .Where(x => x.DateStart >= dateStart 
                        && x.DateStart <= dateEnd)
            .AsNoTracking();
        
        // Подключение опциональных фильтров
        if (locationId != null)
            events = events.Where(e => e.LocationId == locationId);
        if (sponsorId != null)
            events = events.Where(e => e.EventParticipants.Any(x => x.UserId == sponsorId
                                                    && (x.UserRole == EventRole.Manager || x.UserRole == EventRole.Organizer)));
        
        return events.ToListAsync(cancellationToken);
    }

    public Task<Event?> GetEventByIdAsync(long id, CancellationToken cancellationToken)
    {
        return context.Events
            .Include(x => x.Location)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<List<EventUser>> GetEventParticipantsAsync(long eventId, CancellationToken cancellationToken)
    {
        return context.EventUsers
            .Where(x => x.EventId == eventId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public Task<User?> GetSponsorByEventIdAsync(long eventId, CancellationToken cancellationToken)
    {
        return  context.EventUsers
            .Where(x => x.EventId == eventId && (x.UserRole == EventRole.Organizer || x.UserRole == EventRole.Manager))
            .Select(x => x.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
}