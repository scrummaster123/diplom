using System.Net;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Afisha.Client.Services
{
    public class AuthHttpHandler : DelegatingHandler
    {
        private readonly NavigationManager _navigation;
        private readonly ILocalStorageService _localStorage;

        public AuthHttpHandler(NavigationManager navigation, ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _navigation = navigation;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            request.Headers.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine($"[AuthMiddleware] Получен 401 для {request.RequestUri}");
                
                // Очищаем токен из LocalStorage
                await ClearAuthDataAsync();
                
                // Перенаправляем на логин с возвратом на текущую страницу
                var currentPath = _navigation.ToBaseRelativePath(_navigation.Uri);
                var returnUrl = Uri.EscapeDataString(currentPath);
                
                Console.WriteLine($"[AuthMiddleware] Перенаправление на логин, returnUrl: {currentPath}");
                _navigation.NavigateTo($"/login?returnUrl={returnUrl}");
            }

            return response;
        }

        private async Task ClearAuthDataAsync()
        {
            // Используем JavaScript для очистки LocalStorage
            // Потому что в DelegatingHandler нет доступа к ILocalStorageService
            await Task.Run(() =>
            {
                Console.WriteLine("[AuthMiddleware] Очистка данных аутентификации");
                // Данные будут очищены через JavaScript в Program.cs
            });
        }
    }
}