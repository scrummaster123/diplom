using Microsoft.AspNetCore.Components;

namespace Afisha.Client.Services;

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;


public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage, NavigationManager navigation)
    {
        _localStorage = localStorage;
        _navigation = navigation;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        
        if (string.IsNullOrEmpty(token))
        {
            // Возвращаем неавторизованного пользователя
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Создаем авторизованного пользователя
        var user = await _localStorage.GetItemAsync<AuthorizedUserModel>("currentUser");
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, user?.Login ?? "Unknown"),
            new Claim(ClaimTypes.Email, user?.Email ?? ""),
        }, "custom");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task MarkUserAsAuthenticated(string token, AuthorizedUserModel user)
    {
        await _localStorage.SetItemAsync("authToken", token);
        await _localStorage.SetItemAsync("currentUser", user);
        
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Email, user.Email),
        }, "custom");

        var authState = new AuthenticationState(new ClaimsPrincipal(identity));
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("currentUser");
        
        var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
        
        _navigation.NavigateTo("/login");
    }
}