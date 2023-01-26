namespace Dairy.ViewModels;

public record NoteViewModel(long Id, DateTime Date, string Text)
{
    /// <summary>
    /// <remarks>Used only by Dapper</remarks>
    /// </summary>
    private NoteViewModel(): this(default, default, string.Empty) { }
}
public record CreateNoteViewModel(string Text);