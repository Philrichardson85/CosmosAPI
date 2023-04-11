using Azure.Core;
using CosmosAPI.Controllers;
using CosmosAPI.Models;
using CosmosAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Reflection;

namespace CosmosTests
{
    [TestFixture]
    public class ServiceTests
    {
        public ICosmosService _cosmosService;
        private Mock<IOptions<ConnectionStringsSection>> _mockAppSettings;

        [SetUp]
        public void Setup()
        {
            _mockAppSettings = new Mock<IOptions<ConnectionStringsSection>>();

            _mockAppSettings.Setup(x => x.Value).Returns(new ConnectionStringsSection
            {
                CosmosEndpoint = "https://localhost:8081/",
                CosmosKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                ApplicationName = "CosmosAPI"
            });
            _cosmosService = new CosmosService(_mockAppSettings.Object);

        }

        [Test]
        public async Task CosmosService_GetNoteById()
        {
            // Arrange
            string id = "1234";
            // Act
            var resp = await _cosmosService.GetNoteById(id);

            // Assert
            Assert.AreNotEqual("89", resp.Id);
            Assert.AreEqual("1234", resp.Id);
        }

        [Test]
        public async Task CosmosService_GetNotes()
        {
            // Act
            var resp = await _cosmosService.GetNotes();

            // Assert
            Assert.IsNotNull(resp);
            Assert.IsInstanceOf<IEnumerable<Note>>(resp);
            
        }

        [Test]
        public async Task CosmosService_PutNotes()
        {
            // Arrange
            Random random = new Random();
            int randomNumber = random.Next(200);
            Note note = new Note { Id = "5678", DateCreated = DateTime.Now, Tags = new List<string> { "1", "2", "3" }, Text = $"test{randomNumber} text" };
            var old_note = await _cosmosService.GetNoteById(note.Id);
            // Act
            var resp = await _cosmosService.PutNote(note);

            // Assert
            Assert.AreEqual(note.Id, resp.Id);
            Assert.AreNotEqual(old_note.Text, resp.Text,"Text changed");
            Assert.IsInstanceOf<Note>(resp);
        }
        [Test]
        public async Task CosmosService_PostNotes()
        {
            // Arrange
            Note note = new Note { Id = "123456789", DateCreated = DateTime.Now, Tags = new List<string> { "1", "1", "1" }, Text = "test1 text" };
            if (await _cosmosService.GetNoteById(note.Id) != null)
            {
                await _cosmosService.DeleteNote(note.Id);
            }

            // Act
            var resp = await _cosmosService.PostNote(note);

            // Assert
            Assert.AreEqual(note.Id, resp.Id);
            Assert.IsInstanceOf<Note>(resp);
        }
        [Test]
        public async Task CosmosService_DeleteNotes()
        {
            // Arrange
            Note note = new Note { Id = "123456789", DateCreated = DateTime.Now, Tags = new List<string> { "1", "1", "1" }, Text = "test1 text" };

            // Act
            var resp = await _cosmosService.DeleteNote(note.Id);

            // Assert
            Assert.AreEqual($"Deleted Note [{note.Id},{note.Id}]\n", resp);
            Assert.IsInstanceOf<string>(resp);
        }
    }
}