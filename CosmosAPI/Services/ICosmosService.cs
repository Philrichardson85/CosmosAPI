using CosmosAPI.Models;

namespace CosmosAPI.Services
{
    public interface ICosmosService
    {
        Task<IEnumerable<Note>> GetNotes();
        Task<Note> GetNoteById(string id);
        Task<Note> PutNote(Note put_note);
        Task<Note> PostNote(Note post_note);
        Task<string> DeleteNote(string id);
    }
}
