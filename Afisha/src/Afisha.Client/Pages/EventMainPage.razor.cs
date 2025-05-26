using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Pages;

public partial class EventMainPage : ComponentBase
{
    public class EventModel
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool IsOpenToRegister { get; set; }
        public decimal Price { get; set; }
        public bool IsVacation { get; set; }
    }

    protected DateTime? StartDate { get; set; }
    protected DateTime? EndDate { get; set; }
    protected bool IsVacation { get; set; }

    // Пример событий
    protected List<EventModel> AllEvents = new()
    {
        new() { Title = "Event1", Date = DateTime.Today.AddDays(3), IsOpenToRegister = true, Price = 1000, IsVacation = true },
        new() { Title = "Event2", Date = DateTime.Today.AddDays(7), IsOpenToRegister = false, Price = 500, IsVacation = false },
        new() { Title = "Event3", Date = DateTime.Today.AddDays(10), IsOpenToRegister = true, Price = 0, IsVacation = true },
        new() { Title = "Event4", Date = DateTime.Today.AddDays(14), IsOpenToRegister = false, Price = 2000, IsVacation = false }
    };

    protected List<EventModel> FilteredEvents => AllEvents
        .Where(e => (!StartDate.HasValue || e.Date >= StartDate)
                    && (!EndDate.HasValue || e.Date <= EndDate)
                    && (!IsVacation || e.IsVacation))
        .ToList();
}