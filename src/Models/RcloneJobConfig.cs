using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class RcloneJobConfig
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("source_path")]
        public string SourcePath { get; set; }

        [JsonPropertyName("dest_path")]
        public string DestPath { get; set; }

        [JsonPropertyName("log")]
        public string Log { get; set; }

        [JsonPropertyName("max_runtime_minutes")]
        public int MaxRuntimeMinutes { get; set; }
    }
}