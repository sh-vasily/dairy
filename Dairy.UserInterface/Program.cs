using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Dairy;
using Dairy.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiConfiguration = builder.Configuration.GetSection("ApiConfiguration").Get<ApiConfiguration>();

builder.Services
    .AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddScoped<INoteService, NoteService>()
    .AddSingleton(apiConfiguration);

await builder.Build().RunAsync();