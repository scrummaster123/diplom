using System.Net.Http.Json;
using Afisha.Application.DTO.Outputs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Afisha.Client.Pages
{
    public partial class EventDetail : ComponentBase
    {
        [Parameter] public int EventId { get; set; }
        [Parameter] public int UserId { get; set; } = 2;
        

        protected OutputEvent? eventDetails;
        private bool isLoading = true;

        [Inject] private ISnackbar Snackbar { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] protected HttpClient Http { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                isLoading = true;
                
                var baseUrl = "http://localhost:5182";http://localhost:5182/Event?id=1
                var url = $"{baseUrl}/Event/{EventId}";
                eventDetails = await Http.GetFromJsonAsync<OutputEvent>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                Snackbar.Add("Не удалось загрузить информацию о мероприятии", Severity.Error);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task RegisterToEvent()
        {
            try
            {
                var result = await Http.PostAsJsonAsync($"http://localhost:5182/Event/request-approval?eventId={EventId}&userId={UserId}", new { });

                var str = result.Content;
                // Логика регистрации на мероприятие
                // await Http.PostAsJsonAsync($"http://localhost:5182/Event/register/{EventId}", new { });

                Snackbar.Add("Вы успешно зарегистрировались на мероприятие!", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Ошибка: {ex.Message}", Severity.Error);
            }
        }
    }
}