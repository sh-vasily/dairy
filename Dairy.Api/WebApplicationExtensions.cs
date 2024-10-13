using Dairy.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dairy.Api;

public static class WebApplicationExtensions
{
    public static WebApplication MapNoteEndpoints(this WebApplication app)
    {
        app.MapGet("note/all", async ([FromQuery] DateOnly? dateTime, INoteRepository noteRepository, CancellationToken cancellationToken) 
            => await noteRepository.FindAll(dateTime, cancellationToken));
        app.MapGet("note/{id:int}", async (INoteRepository noteRepository, int id, CancellationToken cancellationToken) 
            => await noteRepository .Find(id, cancellationToken));
        app.MapPost("note", async (INoteRepository noteRepository, CreateNoteViewModel note, CancellationToken cancellationToken)
            => await noteRepository.Add(note, cancellationToken));
        app.MapPut("note", async (INoteRepository noteRepository, NoteViewModel note, CancellationToken cancellationToken) 
            => await noteRepository.Update(note, cancellationToken));
        app.MapDelete("note/{noteId:int}", async (INoteRepository noteRepository, int noteId, CancellationToken cancellationToken) 
            => await noteRepository.Delete(noteId, cancellationToken));

        return app;
    }
}