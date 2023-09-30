using System.Net.Http.Json;
using System.Text;
using Dairy.ViewModels;
using Newtonsoft.Json;

namespace Dairy;

public class NoteService : INoteService
{
    private const string ApiUrl = "http://localhost:5555";
    private readonly HttpClient _httpClient;

    public NoteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Add(CreateNoteViewModel noteViewModel)
    {
        var payload = JsonConvert.SerializeObject(noteViewModel);
        using var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        await _httpClient.PostAsync($"{ApiUrl}/Note", httpContent);
    }

    public async Task Update(NoteViewModel noteViewModel)
    {
        var payload = JsonConvert.SerializeObject(noteViewModel);
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        await _httpClient.PutAsync($"{ApiUrl}/Note", httpContent);
    }

    public async Task Delete(int noteId) 
        => await _httpClient.DeleteAsync($"{ApiUrl}/Note/{noteId}");

    public Task<NoteViewModel> Find(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<NoteViewModel>> FindAll() 
        => _httpClient.GetFromJsonAsync<List<NoteViewModel>>($"{ApiUrl}/note/all");
}

public class InMemoryNoteService : INoteService
{
    private long currentId = 0;
    private NoteViewModel[] _notes = new NoteViewModel[1];

    public Task Add(CreateNoteViewModel noteViewModel)
    {
        if (currentId == _notes.Length)
        {
            var newArray = new NoteViewModel[_notes.Length * 2];
            _notes.CopyTo(newArray, 0);
            _notes = newArray;
        }

        _notes[currentId] = new(currentId++, DateTime.Now, noteViewModel.Text);
        return Task.CompletedTask;
    }

    public Task Update(NoteViewModel noteViewModel)
    {
        _notes[noteViewModel.Id] = _notes[noteViewModel.Id] with { Text = noteViewModel.Text };
        return Task.CompletedTask;
    }

    public Task Delete(int noteId)
    {
        _notes[noteId] = null;
        return Task.CompletedTask;
    }

    public Task<NoteViewModel> Find(int id)
    {
        return Task.FromResult(_notes[id]);
    }

    public Task<List<NoteViewModel>> FindAll()
    {
        return Task.FromResult(_notes.ToList());
    }
}