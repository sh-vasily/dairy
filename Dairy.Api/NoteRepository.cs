using System.Data.SQLite;
using Dairy.ViewModels;
using Dapper;

namespace Dairy.Api;

public interface INoteRepository
{
    Task Add(CreateNoteViewModel noteViewModel, CancellationToken cancellationToken);
    Task Update(NoteViewModel noteViewModel, CancellationToken cancellationToken);
    Task Delete(int noteId, CancellationToken cancellationToken);
    Task<NoteViewModel> Find(int id, CancellationToken cancellationToken);
    Task<IEnumerable<NoteViewModel>> FindAll(DateOnly? dateTime, CancellationToken cancellationToken);
}

public class NoteRepository(DatabaseConfig databaseConfig) : INoteRepository
{
    private const string UpdateQuery = @"
            update note
            set text = @Text, updated_at = @UdatedAt
            where id = @Id;
            ";
    
    private const string InsertQuery = @"
            insert into note (text, date)
            values(@Text, @Date);
            ";

    private const string DeleteQuery = @"
            delete from note
            where note.id = @noteId;
            ";

    private const string SelectAllNotesQuery = @"
            select note.id, note.date, note.text
            from note
            order by note.date;
            ";
    
    private const string SelectByDateNotesQuery = @"
            select note.id, note.date, note.text
            from note
            where date(note.date) = date(@date)
            order by note.date;
            ";

    private const string SelectByIdQuery = @"
            select note.id, note.text
            from note
            where note.id = @id;
            ";
    
    private SQLiteConnection SqLiteConnection => 
        new SQLiteConnection($"Data Source={databaseConfig.Path};Version=3;New=True;Compress=True;")
            .Also(connection => connection.Open());

    //todo: try to use merge here
    public async Task Update(NoteViewModel noteViewModel, CancellationToken cancellationToken)
    {
        var queryParams = new
        {
            noteViewModel.Id,
            noteViewModel.Text,
            UpdatedAt = DateTime.Now
        };

        await SqLiteConnection
            .ExecuteAsync(new CommandDefinition(UpdateQuery, queryParams, cancellationToken: cancellationToken));
    }

    public async Task Add(CreateNoteViewModel noteViewModel, CancellationToken cancellationToken)
    {
        var queryParams = new
        {
            noteViewModel.Text,
            Date = DateTime.Now,
        }; 
        
        await SqLiteConnection
            .ExecuteAsync(new CommandDefinition(InsertQuery, queryParams, cancellationToken: cancellationToken));    
    }

    public async Task Delete(int noteId, CancellationToken cancellationToken)
    {
        var queryParams = new
        {
            noteId
        };

        await SqLiteConnection
            .ExecuteAsync(new CommandDefinition(DeleteQuery, queryParams, cancellationToken: cancellationToken));
    }

    public async Task<NoteViewModel> Find(int id, CancellationToken cancellationToken)
    {
        var queryParams = new { id };
        return await SqLiteConnection
            .QueryFirstAsync<NoteViewModel>(new CommandDefinition(SelectByIdQuery, queryParams, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<NoteViewModel>> FindAll(DateOnly? dateTime, CancellationToken cancellationToken)
    {
        if (dateTime is null)
        {
            return await SqLiteConnection
                .QueryAsync<NoteViewModel>(new CommandDefinition(SelectAllNotesQuery, cancellationToken: cancellationToken)); 
        }
        
        var queryParams = new { date = dateTime.Value.ToString("yyyy-MM-dd") };
        
        return await SqLiteConnection
            .QueryAsync<NoteViewModel>(new CommandDefinition(SelectByDateNotesQuery, queryParams, cancellationToken: cancellationToken));
    } 
}
