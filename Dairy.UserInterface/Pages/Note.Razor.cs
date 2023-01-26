using System.Text;
using Dairy.ViewModels;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Dairy.Pages;

public partial class Note
{
    [Parameter]
    public long? NoteId { get; set; }

    [Inject] 
    private HttpClient HttpClient { get; set; }

    private async Task OnChange(string noteContent, string textarea)
    {
        var noteViewModel = new NoteViewModel(NoteId.Value, DateTime.UtcNow, noteContent);
        var payload = JsonConvert.SerializeObject(noteViewModel);
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        await HttpClient.PutAsync("http://localhost:5555/Note", httpContent);
    }
}