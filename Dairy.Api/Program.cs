using Dairy.Api;

var builder = WebApplication.CreateBuilder(args);
var dbPath = builder.Configuration.GetSection("DbPath").Value ?? "C:\\Users\\User\\Documents\\database.db";

// Add services to the container.
builder.Services.AddSingleton(new DatabaseConfig(dbPath));
builder.Services.AddSingleton<INoteRepository, NoteRepository>();
builder.Services.AddControllers();

var app = builder.Build();
app.MapNoteEndpoints();
app.Run();