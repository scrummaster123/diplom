using System.ComponentModel.DataAnnotations;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Afisha.Client.Services
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticatedAsync();
        Task<string?> GetTokenAsync();
        Task<AuthorizedUserModel?> GetCurrentUserAsync();
        Task<bool> LoginAsync(LoginUserModel loginModel);
        Task LogoutAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigation;
        private readonly HttpClient _httpClient;
        private readonly CustomAuthenticationStateProvider _authStateProvider;
        
        private const string TOKEN_KEY = "authToken";
        private const string USER_KEY = "currentUser";
        
        // URL вашего бэкенда - замените на свой!
        private const string API_BASE_URL = "https://localhost:7001/api"; // Или ваш реальный URL

        public AuthService(
            ILocalStorageService localStorage, 
            NavigationManager navigation,
            HttpClient httpClient,
            CustomAuthenticationStateProvider authStateProvider)
        {
            _localStorage = localStorage;
            _navigation = navigation;
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(TOKEN_KEY);
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>(TOKEN_KEY);
        }

        public async Task<AuthorizedUserModel?> GetCurrentUserAsync()
        {
            return await _localStorage.GetItemAsync<AuthorizedUserModel>(USER_KEY);
        }

        public async Task<bool> LoginAsync(LoginUserModel loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{API_BASE_URL}/auth/login", loginModel);
                
                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    
                    if (authResponse != null)
                    {
                        var user = new AuthorizedUserModel
                        {
                            Login = "Jujusko", // Ваш логин
                            Email = loginModel.Email,
                        };

                        // Сохраняем в LocalStorage
                        await _localStorage.SetItemAsync(TOKEN_KEY, authResponse.Token);
                        await _localStorage.SetItemAsync(USER_KEY, user);
                        
                        // Обновляем состояние аутентификации
                        await _authStateProvider.MarkUserAsAuthenticated(authResponse.Token, user);
                        
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync(TOKEN_KEY);
            await _localStorage.RemoveItemAsync(USER_KEY);
            
            await _authStateProvider.MarkUserAsLoggedOut();
        }
    }

    // Модели данных
    public class AuthorizedUserModel
    {
        public string Login { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Login { get; set; } = "";
    }
    
    public record LoginUserModel(    
        [Required(ErrorMessage = "Email обязателен для заполнения")] 
        [EmailAddress(ErrorMessage = "Некорректный формат email")] 
        string Email,
        
        [Required(ErrorMessage = "Пароль обязателен для заполнения")] 
        string Password
    );
}