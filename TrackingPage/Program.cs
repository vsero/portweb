using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
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

await builder.Build().RunAsync();