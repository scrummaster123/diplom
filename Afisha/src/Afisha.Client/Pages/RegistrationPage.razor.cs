using System.Net.Http.Json;
using Afisha.Application.DTO.Inputs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Afisha.Client.Pages;

public partial class RegistrationPage : ComponentBase
{
    private MudForm form;
    private RegistrationUserModel registrationModel = new();
    private bool isFormValid;
    private bool isSubmitting;
    
    // Для управления видимостью паролей
    private bool isPasswordVisible;
    private InputType passwordInput = InputType.Password;
    private string passwordInputIcon = Icons.Material.Filled.VisibilityOff;
    
    private bool isConfirmPasswordVisible;
    private InputType confirmPasswordInput = InputType.Password;
    private string confirmPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

    private async Task Submit()
    {
        if (!isFormValid)
        {
            Snackbar.Add("Пожалуйста, исправьте ошибки в форме", Severity.Error);
            return;
        }

        isSubmitting = true;
        StateHasChanged();

        try
        {
            // Замените URL на ваш API endpoint
            var response = await HttpClient.PostAsJsonAsync("http://localhost:5182/Auth/user-registration", registrationModel);

            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Регистрация прошла успешно!", Severity.Success);
                Navigation.NavigateTo("/events");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Snackbar.Add($"Ошибка регистрации: {errorMessage}", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Произошла ошибка: {ex.Message}", Severity.Error);
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

    private void ToggleConfirmPasswordVisibility()
    {
        if (isConfirmPasswordVisible)
        {
            isConfirmPasswordVisible = false;
            confirmPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            confirmPasswordInput = InputType.Password;
        }
        else
        {
            isConfirmPasswordVisible = true;
            confirmPasswordInputIcon = Icons.Material.Filled.Visibility;
            confirmPasswordInput = InputType.Text;
        }
    }
}