using Newtonsoft.Json;

namespace CosmosAPI.Models
{
    public class Note
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Text { get; set; }
        public List<string>? Tags { get; set; }
    }
}
