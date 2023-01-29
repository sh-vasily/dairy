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

    [Inject] 
    private HttpClient HttpClient { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        var noteViewModel = await HttpClient.GetFromJsonAsync<NoteViewModel>(
            $"http://localhost:5555/Note/{NoteId}");

        Text = noteViewModel.Text;
    }

    private async Task OnChange(string noteContent, string textarea)
    {
        Text = noteContent;
        var noteViewModel = new NoteViewModel(NoteId.Value, DateTime.UtcNow, noteContent);
        var payload = JsonConvert.SerializeObject(noteViewModel);
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        await HttpClient.PutAsync("http://localhost:5555/Note", httpContent);
    }
}