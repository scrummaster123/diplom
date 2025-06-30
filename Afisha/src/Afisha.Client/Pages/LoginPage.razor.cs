using System.Net.Http.Json;
using Afisha.Application.DTO.Inputs;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Afisha.Client.Pages;

public partial class LoginPage : ComponentBase
{
    public class ClientLoginUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    private MudForm form;
    private ClientLoginUserModel loginModel;
    private bool isFormValid;
    private bool isSubmitting;
    private bool rememberMe;
    private string errorMessage = string.Empty;
    
    // Для управления видимостью пароля
    private bool isPasswordVisible;
    private InputType passwordInput = InputType.Password;
    private string passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private ILocalStorageService LocalStorageService { get; set; }


    protected override async Task OnInitializedAsync()
    {
        // Проверяем, если пользователь уже авторизован
        // можно перенаправить на главную страницу
        FillTestData();
        await base.OnInitializedAsync();
    }

    private async Task Submit()
    {
        errorMessage = string.Empty;

        if (!isFormValid)
        {
            Snackbar.Add("Пожалуйста, заполните все обязательные поля", Severity.Warning);
            return;
        }

        isSubmitting = true;
        StateHasChanged();

        try
        {
            // Замените URL на ваш API endpoint для авторизации
            var response = await HttpClient.PostAsJsonAsync("http://localhost:5182/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                // Успешная авторизация
                var result = await response.Content.ReadAsStringAsync();

                // Здесь можно сохранить токен в localStorage или cookies
                await LocalStorageService.SetItemAsync("authToken", result);
                
                Snackbar.Add("Вход выполнен успешно!", Severity.Success);
                
                // Перенаправляем на главную страницу или на страницу, с которой пришел пользователь
                var returnUrl = Navigation.Uri.Contains("returnUrl") 
                    ? ExtractReturnUrl() 
                    : "/";
                Navigation.NavigateTo(returnUrl);
            }
            else
            {
                // Обработка ошибок авторизации
                var errorContent = await response.Content.ReadAsStringAsync();
                
                errorMessage = response.StatusCode switch
                {
                    System.Net.HttpStatusCode.Unauthorized => "Неверный email или пароль",
                    System.Net.HttpStatusCode.BadRequest => "Некорректные данные для входа",
                    System.Net.HttpStatusCode.TooManyRequests => "Слишком много попыток входа. Попробуйте позже",
                    _ => $"Ошибка входа: {errorContent}"
                };
                
                StateHasChanged();
            }
        }
        catch (HttpRequestException ex)
        {
            errorMessage = "Ошибка подключения к серверу. Проверьте интернет-соединение";
            Snackbar.Add(errorMessage, Severity.Error);
        }
        catch (Exception ex)
        {
            errorMessage = $"Произошла неожиданная ошибка: {ex.Message}";
            Snackbar.Add(errorMessage, Severity.Error);
        }
        finally
        {
            isSubmitting = false;
            StateHasChanged();
        }
    }

    private void TogglePasswordVisibility()
    {
        if (isPasswordVisible)
        {
            isPasswordVisible = false;
            passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            passwordInput = InputType.Password;
        }
        else
        {
            isPasswordVisible = true;
            passwordInputIcon = Icons.Material.Filled.Visibility;
            passwordInput = InputType.Text;
        }
    }

    private string ExtractReturnUrl()
    {
        var uri = new Uri(Navigation.Uri);
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        return query.TryGetValue("returnUrl", out var returnUrl) ? returnUrl.ToString() : "/";
    }

    // Дополнительный метод для автозаполнения (для тестирования)
    private void FillTestData()
    {
        loginModel = new ClientLoginUserModel
        {
            Email = "test@mail.ru",
            Password = "123456"
        };
        StateHasChanged();
    }
}
