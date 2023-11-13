using System.Net.Http.Json;
using System.Text;
using Dairy.ViewModels;
using Newtonsoft.Json;

namespace Dairy;

public class NoteService : INoteService
{
    private const string ApiUrl = "note";
    private readonly HttpClient _httpClient;

    public NoteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Add(CreateNoteViewModel noteViewModel)
    {
        var payload = JsonConvert.SerializeObject(noteViewModel);
        using var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        await _httpClient.PostAsync($"{ApiUrl}", httpContent);
    }

    public async Task Update(NoteViewModel noteViewModel)
    {
        var payload = JsonConvert.SerializeObject(noteViewModel);
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        await _httpClient.PutAsync($"{ApiUrl}", httpContent);
    }

    public async Task Delete(int noteId) 
        => await _httpClient.DeleteAsync($"{ApiUrl}/{noteId}");

    public async Task<NoteViewModel> Find(int id)
    {
        return await _httpClient.GetFromJsonAsync<NoteViewModel>(
            $"note/{id}");
    }

    public Task<List<NoteViewModel>> FindAll() 
        => _httpClient.GetFromJsonAsync<List<NoteViewModel>>($"{ApiUrl}/all");
    
    public Task<List<NoteViewModel>> FindByDate(DateTime dateTime) 
        => _httpClient.GetFromJsonAsync<List<NoteViewModel>>($"{ApiUrl}/all?dateTime={dateTime.ToString("yyyy-MM-dd")}");
}