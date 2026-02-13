using System.Text.Json.Serialization;

namespace AngularWithASP.Server.Models
{
    public class UpdateNodeDto
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        // Possibili forme inviate dal client
        [JsonPropertyName("Checked")]
        public string? Checked { get; set; } = null;

        [JsonPropertyName("expanded")]
        public bool? expanded { get; set; } = null;

        //[JsonPropertyName("checked")]
        //public string? @checked { get; set; }
    }
}