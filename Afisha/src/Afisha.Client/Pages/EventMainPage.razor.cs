using System.Net;
using System.Net.Http.Json;
using System.Text;
using Afisha.Application.DTO.Outputs;
using Afisha.Client.Events;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Afisha.Client.Pages;

public partial class EventMainPage : ComponentBase
{
    protected bool IsVacation { get; set; }
   

    [Inject] protected HttpClient Http { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }


    protected List<OutputEvent> AllEvents { get; set; } = new();
    protected List<OutputEvent> FilteredEvents { get; set; } = new();

    private DateRange _dateRange = new DateRange(DateTime.Now.Date, DateTime.Now.AddDays(5).Date);

    protected DateRange CurrentDateRange => _dateRange;

    public async Task SetDateRange(DateRange value)
    {
        _dateRange = value;
        await ApplyFilters();
    }
        
    protected List<OutputLocationBase> Locations { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        await GetEvents();
    }

    private async Task GetEvents()
    {
        var baseUrl = "http://localhost:5182";
        var endpointEvents = "/Event/filtered-events";

        var queryEvents = new StringBuilder("?");
        if (CurrentDateRange.Start.HasValue)
            queryEvents.Append($"dateStart={CurrentDateRange.Start.Value:yyyy-MM-dd}&dateEnd={CurrentDateRange.End:yyyy-MM-dd}");
        Console.WriteLine(_dateRange.End);
        Console.WriteLine(_dateRange.Start);
        var urlForEvents = $"{baseUrl}{endpointEvents}{queryEvents}";

        try
        {
            AllEvents = await Http.GetFromJsonAsync<List<OutputEvent>>(urlForEvents);
        }
        catch (Exception ex)
        {
            // Обработка ошибок (можно вывести пользователю)
            Console.WriteLine($"Ошибка загрузки событий: {ex.Message}");
            AllEvents = new List<OutputEvent>(); // или как-то иначе обработать ошибку
        }
    }

    protected async Task ApplyFilters()
    {
        await GetEvents();
        FilteredEvents = AllEvents
            .Where(e =>
                (!CurrentDateRange.Start.HasValue || e.DateStart.ToDateTime(TimeOnly.MinValue) >= CurrentDateRange.Start.Value) &&
                (!CurrentDateRange.End.HasValue || e.DateStart.ToDateTime(TimeOnly.MinValue) <= CurrentDateRange.End.Value) &&
                (!IsVacation || e.IsOpenToRegister == IsVacation))
            .ToList();
        StateHasChanged();
    }
    
    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private EventCallback<DataGridRowClickEventArgs<OutputEvent>> OnRowClickHandler => 
        EventCallback.Factory.Create<DataGridRowClickEventArgs<OutputEvent>>(this, RowClickEvent);
    private void RowClickEvent(DataGridRowClickEventArgs<OutputEvent> args)
    {
        // Навигация на страницу деталей мероприятия
        // Предполагается, что у EventModel есть свойство Id
        var eventId = args.Item.Id;
        NavigationManager.NavigateTo($"/event/{eventId}");
    }
    
    [Inject] protected IDialogService DialogService { get; set; }

    private async Task OnCreateClickHandler()
    {
        var dialog = DialogService.Show<CreateEventPopup>("Создать мероприятие");
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await ApplyFilters();
        }
    }
    
    private async Task OnIsVacationChanged(bool value)
    {
        IsVacation = value;
        await ApplyFilters();
    }
}