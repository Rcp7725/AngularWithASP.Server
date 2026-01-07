using System.Text.Json.Serialization;

namespace AngularWithASP.Server.Models
{
    public class UpdateCheckboxDto
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        // Possibili forme inviate dal client
        [JsonPropertyName("Checked")]
        public string? Checked { get; set; }

        //[JsonPropertyName("checked")]
        //public string? @checked { get; set; }
    }
}