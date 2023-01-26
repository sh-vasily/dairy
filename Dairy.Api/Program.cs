using System.Data.SQLite;
using Dairy.Api;
using Dairy.ViewModels;
using Dapper;
using INoteService = Dairy.Api.INoteService;

var builder = WebApplication.CreateBuilder(args);

var dbPath = builder.Configuration.GetSection("DbPath").Value ?? "C:\\Users\\User\\Documents\\database.db";
Console.WriteLine($"Database Path = {dbPath}");
var dbConnection = new SQLiteConnection(@$"Data Source={dbPath};Version=3;New=True;Compress=True;");
dbConnection.Open();
await dbConnection.ExecuteAsync(@"
    create table if not exists note(id INTEGER PRIMARY KEY AUTOINCREMENT, text TEXT);
        ");

var pragmaResult = await dbConnection.QueryFirstAsync(@"
    select user_version 
    from pragma_user_version();
");

if (pragmaResult is IDictionary<string, object> pairs
    && pairs.TryGetValue("user_version", out var userVersion)
    && (long)userVersion < 1)
{
    Console.WriteLine("Migrate date column starting...");
    await dbConnection.ExecuteAsync(@"
        alter table note add column date datetime;
        update note
        set date = datetime('now')
        where date is null;
        pragma user_version = 1;
    ");
    Console.WriteLine("Migrate date column finished...");
}

// Add services to the container.
builder.Services.AddSingleton<INoteService, NoteService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowCorsPolicy", x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("note/all", async (INoteService noteService) => await noteService.FindAll(dbPath));
app.MapGet("note/{id:int}", async (INoteService noteService, int id) => await noteService.Find(id, dbPath));
app.MapPost("note", async (INoteService noteService, CreateNoteViewModel note) => await noteService.Add(note, dbPath));
app.MapPut("note", async (INoteService noteService, NoteViewModel note) => await noteService.Update(note));
app.MapDelete("note/{noteId:int}", async (INoteService noteService, int noteId) => await noteService.Delete(noteId, dbPath));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();