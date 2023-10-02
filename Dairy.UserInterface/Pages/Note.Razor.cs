using System.Net.Http.Json;
using System.Text;
using Dairy.ViewModels;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Dairy.Pages;

public partial class Note
{
    [Parameter]
    public long? NoteId { get; set; }
    
    public string Text { get; set; }

    [Inject] INoteService NoteService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        var noteViewModel = await NoteService.Find((int)NoteId.Value);

        Text = noteViewModel.Text;
    }

    private async Task OnChange(string noteContent, string textarea)
    {
        Text = noteContent;
        var noteViewModel = new NoteViewModel(NoteId.Value, DateTime.UtcNow, noteContent);
        await NoteService.Update(noteViewModel);
    }
}