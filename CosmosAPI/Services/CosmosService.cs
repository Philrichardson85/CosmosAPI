using CosmosAPI.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Net;

namespace CosmosAPI.Services
{
    public class CosmosService : ICosmosService
    {
        private CosmosClient cosmosClient;
        private Database database;
        private Microsoft.Azure.Cosmos.Container container;
        private readonly ConnectionStringsSection _appSettings;

        public CosmosService(IOptions<ConnectionStringsSection> _appSettings) {
            this.cosmosClient = new CosmosClient(
                _appSettings.Value.CosmosEndpoint,
                _appSettings.Value.CosmosKey,
                new CosmosClientOptions() { ApplicationName = _appSettings.Value.ApplicationName });

            this.container = cosmosClient.GetContainer("myNotesDB", "myNotes");
        }

        async Task<Note> ICosmosService.GetNoteById(string id)
        {
            try
            {
                ItemResponse<Note> noteResponse = await this.container.ReadItemAsync<Note>(id, new Microsoft.Azure.Cosmos.PartitionKey(id));
                return noteResponse.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
        
        async Task<IEnumerable<Note>> ICosmosService.GetNotes()
        {
            var sqlQueryText = $"SELECT * FROM myNotes";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Note> queryResultSetIterator = this.container.GetItemQueryIterator<Note>(queryDefinition);

            List<Note> notes = new List<Note>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Note> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Note note in currentResultSet)
                {
                    notes.Add(note);
                    Console.WriteLine("\tRead {0}\n", notes);
                }
            }
            return notes;
        }

        async Task<string> ICosmosService.DeleteNote(string id)
        {
            var partitionKeyValue = id;
            var noteId = id;

            ItemResponse<Note> noteResponse = await this.container.DeleteItemAsync<Note>(noteId, new Microsoft.Azure.Cosmos.PartitionKey(partitionKeyValue));
            Console.WriteLine("Deleted Note [{0},{1}]\n", partitionKeyValue, noteId);
            return $"Deleted Note [{partitionKeyValue},{noteId}]\n";
        }

        async Task<Note> ICosmosService.PostNote(Note post_note)
        {
            try
            {
                ItemResponse<Note> noteResponse = await this.container.ReadItemAsync<Note>(post_note.Id, new Microsoft.Azure.Cosmos.PartitionKey(post_note.Id));
                Console.WriteLine("Item in database with id: {0} already exists\n", noteResponse.Resource.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ItemResponse<Note> noteResponse = await this.container.CreateItemAsync<Note>(post_note, new Microsoft.Azure.Cosmos.PartitionKey(post_note.Id));
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", noteResponse.Resource.Id, noteResponse.RequestCharge);
                return noteResponse.Resource;
            }

            return null;
        }

        async Task<Note> ICosmosService.PutNote(Note put_note)
        {

            ItemResponse<Note> noteResponse = await this.container.ReadItemAsync<Note>(put_note.Id, new Microsoft.Azure.Cosmos.PartitionKey(put_note.Id));
            Note itemBody = noteResponse.Resource;

            itemBody.DateCreated = DateTime.Now;
            itemBody.Text = put_note.Text;
            itemBody.Tags = put_note.Tags;

            noteResponse = await this.container.ReplaceItemAsync<Note>(itemBody, itemBody.Id, new Microsoft.Azure.Cosmos.PartitionKey(itemBody.Id));
            Console.WriteLine("Updated Note [{0},{1}].\n \tBody is now: {2}\n", itemBody.Id, itemBody.Id, noteResponse.Resource.Text);

            return noteResponse.Resource;
        }
    }
}
