using System.Net.Http.Json;
using System.Text;
using Dairy.ViewModels;
using Newtonsoft.Json;

namespace Dairy;

public class NoteService : INoteService
{
    private readonly string _apiUrl;
    private readonly HttpClient _httpClient;

    public NoteService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiUrl = configuration.GetSection("ApiUrl").Value ?? "api";
    }

    public async Task Add(CreateNoteViewModel noteViewModel)
    {
        var payload = JsonConvert.SerializeObject(noteViewModel);
        using var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        await _httpClient.PostAsync($"{_apiUrl}", httpContent);
    }

    public async Task Update(NoteViewModel noteViewModel)
    {
        var payload = JsonConvert.SerializeObject(noteViewModel);
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        await _httpClient.PutAsync($"{_apiUrl}", httpContent);
    }

    public async Task Delete(int noteId) 
        => await _httpClient.DeleteAsync($"{_apiUrl}/{noteId}");

    public async Task<NoteViewModel> Find(int id)
    {
        return await _httpClient.GetFromJsonAsync<NoteViewModel>(
            $"{_apiUrl}/{id}");
    }

    public Task<List<NoteViewModel>> FindAll() 
        => _httpClient.GetFromJsonAsync<List<NoteViewModel>>($"{_apiUrl}/all");

    public Task<List<NoteViewModel>> FindByDate(DateTime dateTime)
    {
        return _httpClient.GetFromJsonAsync<List<NoteViewModel>>($"{_apiUrl}/all?dateTime={dateTime.ToString("yyyy-MM-dd")}");
    }
         
}