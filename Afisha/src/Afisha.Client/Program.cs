using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Afisha.Client;
using Afisha.Client.Services;
using Blazored.LocalStorage;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// LocalStorage
builder.Services.AddBlazoredLocalStorage();

// HttpClient
// builder.Services.AddScoped(sp => new HttpClient());

// MudBlazor
builder.Services.AddMudServices();


// HTTP Client с AuthHandler
builder.Services.AddScoped<AuthHttpHandler>();
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<AuthHttpHandler>();
    handler.InnerHandler = new HttpClientHandler();
    
    return new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:5182") // Ваш API URL
    };
});

await builder.Build().RunAsync();