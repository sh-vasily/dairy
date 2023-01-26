using Dairy.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Dairy.Pages;

public partial class Notes
{
    [Inject] NavigationManager NavigationManager { get; set; }

    [Inject] private INoteService _noteService { get; set; }

    private NoteViewModel[]? notes;

    protected override async Task OnInitializedAsync() => await RefreshNotes();

    private void OnClick(long noteId)
    {
        NavigationManager.NavigateTo($"/Note/{noteId}");
    }

    private async Task AddNote(string args, string textarea)
    {
        var noteViewModel = new CreateNoteViewModel(args);
        await _noteService.Add(noteViewModel);
        await RefreshNotes();
    }

    private async Task Delete(long noteId)
    {
        await _noteService.Delete((int)noteId);
        await RefreshNotes();
    }

    private async Task RefreshNotes() => notes = (await _noteService.FindAll()).ToArray();
}