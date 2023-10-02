using Dairy.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Dairy.Pages;

public partial class Notes
{
    [Inject] NavigationManager NavigationManager { get; set; }

    [Inject] private INoteService _noteService { get; set; }

    private NoteViewModel[]? notes;

    private CreateNoteViewModel note = new ();

    protected override async Task OnInitializedAsync() => await RefreshNotes();

    private void OnClick(long noteId)
    {
        NavigationManager.NavigateTo($"/Note/{noteId}");
    }
    
    async Task Submit()
    {
        await _noteService.Add(note);
        await RefreshNotes();
        note.Text = string.Empty;
    }

    private async Task Delete(long noteId)
    {
        await _noteService.Delete((int)noteId);
        await RefreshNotes();
    }

    private async Task RefreshNotes() => notes = (await _noteService.FindAll()).ToArray();
}