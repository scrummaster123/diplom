using System.Net.Http.Json;
using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Afisha.Client.Events;

public partial class CreateEventPopup : ComponentBase
{
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

    private MudForm form;
    private CreateEvent eventModel = new CreateEvent();
    private bool isValid;
    
    private DateTime? dateStart;
    [Inject] HttpClient HttpClient { get; set; }
    protected OutputMiniUserModel Author { get; set; }
    
    protected OutputLocationBase SelectedLocation { get; set; }
    protected List<OutputLocationBase> locations { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        // Инициализация модели, если нужно
        eventModel.IsOpenToRegister = true;
        locations = await HttpClient.GetFromJsonAsync<List<OutputLocationBase>>("http://localhost:5182/Location/all") 
                    ?? [];
        Author = new OutputMiniUserModel
        {
            Login = "Ivan",
            FullName = "Петрович",
            Id = 2
        };
    }

    void Cancel() => MudDialog.Cancel();

    private async Task Submit()
    {
        await form.Validate();
        if (isValid)
        {
            eventModel.DateStart = DateOnly.FromDateTime(dateStart.HasValue ? dateStart.Value : DateTime.Now);
            eventModel.SponsorId = Author.Id;
            eventModel.LocationId = SelectedLocation.Id;
            // Отправляем данные на сервер
            var response = await HttpClient.PostAsJsonAsync("http://localhost:5182/Event/create", eventModel);

            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Мероприятие создано", Severity.Success);
                MudDialog.Close(DialogResult.Ok(eventModel));
            }
            else
            {
                Snackbar.Add("Ошибка при создании мероприятия", Severity.Error);
            }
        }
    }
}