var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy(name: "AllowCorsPolicy", x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    })
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder
    .Build();

app.UseCors("AllowCorsPolicy");
app.MapReverseProxy();

app.Run();