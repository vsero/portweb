using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;
using System.Globalization;
using System.Text.Json;
using TrackingPage;
using TrackingPage.Services;
using UTrack.V1;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                 ?? "https://kmtp.info/upapi/hs/utrackv1/";

var xApiKey = builder.Configuration["ApiSettings:XApiKey"] ?? string.Empty;

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl),
    Timeout = TimeSpan.FromSeconds(30)
});

builder.Services.AddScoped(sp => 
    new ApiClient(sp.GetRequiredService<HttpClient>(), xApiKey));

builder.Services.AddScoped<UserContext>();

builder.Services.AddMudServices();

builder.Services.AddLocalization();

WebAssemblyHost host = builder.Build();
await SetupUserContext(host);
await host.RunAsync();


static async Task SetupUserContext(WebAssemblyHost host)
{
    var js = host.Services.GetRequiredService<IJSRuntime>();
    var userContext = host.Services.GetRequiredService<UserContext>();

    // Culture
    userContext.Culture = CultureInfo.CurrentCulture.Name;

    // Telegram
    try
    {
        var jsonData = await js.InvokeAsync<string>("getTelegramData");
        if (!string.IsNullOrEmpty(jsonData))
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
            var tgData = JsonSerializer.Deserialize<TgInitData>(jsonData, options);

            if (tgData?.User != null)
            {
                userContext.IsTg = true;
                userContext.TgUser = tgData.User;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка Telegram SDK: {ex.Message}");
    }

}