using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Ecothon.Web;
using Ecothon.Web.Definitions.Contants;
using Ecothon.Web.HttpMessageHandlers;
using Ecothon.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("appsettings.json", false, true);

if (!builder.HostEnvironment.IsProduction())
{
    builder.Configuration.AddJsonFile($"appsettings.{builder.HostEnvironment.Environment}.json", false, true);
}

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage(
    options =>
    {
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        options.JsonSerializerOptions.WriteIndented = false;
    });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped(sp => (IAuthService)sp.GetRequiredService<AuthenticationStateProvider>());
builder.Services.AddTransient<IStrapiApiService, StrapiApiService>();

builder.Services
    .AddHttpClient(HttpClientConstants.HttpClientNames.API, client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>(ConfigurationConstants.AppSettings.API_BASE_URL)))
    .ConfigurePrimaryHttpMessageHandler(
        serviceProvider => new AuthorizationHttpMessageHandler(async () => await serviceProvider.GetRequiredService<ILocalStorageService>().GetItemAsStringAsync(LocalStorageConstants.ACCESS_TOKEN)));

builder.Services
    .AddHttpClient(HttpClientConstants.HttpClientNames.STRAPI_API, client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>(ConfigurationConstants.AppSettings.Strapi.API_BASE_URL)))
    .ConfigurePrimaryHttpMessageHandler(serviceProvider => new AuthorizationHttpMessageHandler(() => Task.FromResult(serviceProvider.GetRequiredService<IConfiguration>().GetValue<string>(ConfigurationConstants.AppSettings.Strapi.API_TOKEN))));

await builder.Build().RunAsync();
