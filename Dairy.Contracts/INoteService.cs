namespace Dairy.ViewModels;

public interface INoteService
{
    Task Add(CreateNoteViewModel noteViewModel);
    Task Update(NoteViewModel noteViewModel);
    Task Delete(int noteId);
    Task<NoteViewModel> Find(int id);
    Task<List<NoteViewModel>> FindAll();   
    Task<List<NoteViewModel>> FindByDate(DateTime dateTime);   
}