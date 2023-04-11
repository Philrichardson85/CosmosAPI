using CosmosAPI.Models;
using CosmosAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CosmosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly ICosmosService _cosmosService;
        public NoteController(ICosmosService cosmosService)
        {
            this._cosmosService = cosmosService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult> RetrieveNotes()
        {
            IEnumerable<Note> retrievedNotes = await this._cosmosService.GetNotes();
            return Ok(retrievedNotes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> RetrieveNotesById(string id)
        {
            Note retrievedNotes = await this._cosmosService.GetNoteById(id);
            return Ok(retrievedNotes);
        }

        [HttpPut]
        public async Task<ActionResult> PutNotes([FromBody] Note put_note)
        {
            Note retrievedNotes = await this._cosmosService.PutNote(put_note);
            return Ok(retrievedNotes);
        }

        [HttpPost]
        public async Task<ActionResult> PostNotes([FromBody] Note post_note)
        {
            Note retrievedNotes = await this._cosmosService.PostNote(post_note);
            return Ok(retrievedNotes);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteNotes(string id)
        {
            string deletedNote = await this._cosmosService.DeleteNote(id);
            return Ok(deletedNote);
        }

    }
}
