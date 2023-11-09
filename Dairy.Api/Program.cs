using Dairy.Api;
using Dairy.ViewModels;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var dbPath = builder.Configuration.GetSection("DbPath").Value ?? "C:\\Users\\User\\Documents\\database.db";

// Add services to the container.
builder.Services.AddSingleton(new DatabaseConfig(dbPath));
builder.Services.AddSingleton<INoteRepository, NoteRepository>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("note/all", async ([FromQuery] DateOnly? dateTime, INoteRepository noteRepository, CancellationToken cancellationToken) 
    => await noteRepository.FindAll(dateTime, cancellationToken));
app.MapGet("note/{id:int}", async (INoteRepository noteRepository, int id, CancellationToken cancellationToken) 
    => await noteRepository.Find(id, cancellationToken));
app.MapPost("note", async (INoteRepository noteRepository, CreateNoteViewModel note, CancellationToken cancellationToken)
    => await noteRepository.Add(note, cancellationToken));
app.MapPut("note", async (INoteRepository noteRepository, NoteViewModel note, CancellationToken cancellationToken) 
    => await noteRepository.Update(note, cancellationToken));
app.MapDelete("note/{noteId:int}", async (INoteRepository noteRepository, int noteId, CancellationToken cancellationToken) 
    => await noteRepository.Delete(noteId, cancellationToken));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();