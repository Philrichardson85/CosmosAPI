using CosmosAPI.Controllers;
using CosmosAPI.Models;
using CosmosAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Moq;
using Newtonsoft.Json.Linq;

namespace CosmosTests
{
    [TestFixture]
    public class ApiTests
    {
        private Mock<ICosmosService> _cosmosServiceMock;
        private NoteController _notesController;

        [SetUp]
        public void Setup()
        {
            _cosmosServiceMock = new Mock<ICosmosService>();
            _notesController = new NoteController(_cosmosServiceMock.Object);
        }

        [Test]
        public async Task RetrieveNotes_ReturnsOk()
        {
            // Arrange
            var expectedNotes = new List<Note> {
                new Note { Id = Guid.NewGuid().ToString(),
                    DateCreated = DateTime.Now,
                    Tags = new List<string> { "1", "1", "1" }, 
                    Text = "test1 text" }, 
                new Note { Id = Guid.NewGuid().ToString(), 
                    DateCreated = DateTime.Now, 
                    Tags = new List<string> { "2", "2", "2" }, 
                    Text = "test2 text" } 
            };

            _cosmosServiceMock.Setup(cs => cs.GetNotes()).ReturnsAsync(expectedNotes);

            // Act
            var response = await _notesController.RetrieveNotes() as OkObjectResult;

            var myNotes = response.Value as IEnumerable<Note>;

            // Assert
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.IsInstanceOf<List<Note>>(myNotes);
        }

        [Test]
        public async Task RetrieveNotesById_ReturnsOk()
        {
            // Arrange
            var expectedNotes = new Note
            {
                Id = "1234",
                DateCreated = DateTime.Now,
                Tags = new List<string> { "1", "1", "1" },
                Text = "test1 text"
            };

            _cosmosServiceMock.Setup(cs => cs.GetNoteById("1234")).ReturnsAsync(expectedNotes);

            // Act
            var response = await _notesController.RetrieveNotesById("1234") as OkObjectResult;

            var myNote = response.Value as Note;

            // Assert
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.IsInstanceOf<Note>(myNote);
        }

        [Test]
        public async Task PutNote_ReturnsOk()
        {
            // Arrange
            var expectedNote = new Note
            {
                Id = "1234",
                DateCreated = DateTime.Now,
                Tags = new List<string> { "1", "1", "1" },
                Text = "test1 text"
            };

            _cosmosServiceMock.Setup(cs => cs.PutNote(expectedNote)).ReturnsAsync(expectedNote);

            // Act
            var response = await _notesController.PutNotes(expectedNote) as OkObjectResult;

            var myNote = response.Value as Note;

            // Assert
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.IsInstanceOf<Note>(myNote);
        }

        [Test]
        public async Task PostNote_ReturnsOk()
        {
            // Arrange
            var expectedNote = new Note
            {
                Id = "1234",
                DateCreated = DateTime.Now,
                Tags = new List<string> { "1", "1", "1" },
                Text = "test1 text"
            };

            _cosmosServiceMock.Setup(cs => cs.PostNote(expectedNote)).ReturnsAsync(expectedNote);

            // Act
            var response = await _notesController.PostNotes(expectedNote) as OkObjectResult;

            var myNote = response.Value as Note;

            // Assert
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.IsInstanceOf<Note>(myNote);
        }

        [Test]
        public async Task DeleteNote_ReturnsOk()
        {
            // Arrange
            string expectedNote = "1234";
            string expectedResponse = $"Deleted Family [{expectedNote},{expectedNote}]\n";

            _cosmosServiceMock.Setup(cs => cs.DeleteNote(expectedNote)).ReturnsAsync(expectedResponse);

            // Act
            var response = await _notesController.DeleteNotes(expectedNote) as OkObjectResult;

            var resp = response.Value as string;

            // Assert
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.IsInstanceOf<string>(resp);
        }
    }
}