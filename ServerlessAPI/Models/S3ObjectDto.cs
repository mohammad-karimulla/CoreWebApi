using System.Text.Json.Serialization;

namespace ServerlessAPI.Models
{
    public class S3ObjectDto
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("PresignedUrl")]
        public string PresignedUrl { get; set; }
    }
}
