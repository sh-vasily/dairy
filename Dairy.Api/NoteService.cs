using System.Data.SQLite;
using Dairy.ViewModels;
using Dapper;

namespace Dairy.Api;

public interface INoteService
{
    Task Add(CreateNoteViewModel noteViewModel, string dbPath);
    Task Update(NoteViewModel noteViewModel);
    Task Delete(int noteId, string dbPath);
    Task<NoteViewModel> Find(int id, string dbPath);
    Task<IEnumerable<NoteViewModel>> FindAll(string dbPath);
}

public class NoteService : INoteService
{
    public static SQLiteConnection GetSqLiteConnection(string dbPath) =>
        new SQLiteConnection($"Data Source={dbPath};Version=3;New=True;Compress=True;")
            .Also(connection => connection.Open());

    
    public Task Update(NoteViewModel noteViewModel)
    {
        throw new NotImplementedException();
    }

    public async Task Add(CreateNoteViewModel noteViewModel, string dbPath)
    {
        await GetSqLiteConnection(dbPath).ExecuteAsync(@"
            insert into note (text, date)
            values(@Text, @Date);
        ", new
        {
            noteViewModel.Text,
            Date = DateTime.Now,
        });    
    }
    
    public async Task Delete(int noteId, string dbPath) 
        => await GetSqLiteConnection(dbPath).QueryAsync(@"
            delete from note
            where note.id = @noteId;
        ", new { noteId });

    public async Task<NoteViewModel> Find(int id, string dbPath) 
        => await GetSqLiteConnection(dbPath).QueryFirstAsync<NoteViewModel>(@"
            select note.id, note.text
            from note
            where note.id = @id;
        ", id);

    public async Task<IEnumerable<NoteViewModel>> FindAll(string dbPath) 
        => await GetSqLiteConnection(dbPath).QueryAsync<NoteViewModel>(@"
            select note.id, note.date, note.text
            from note;
        ");
}