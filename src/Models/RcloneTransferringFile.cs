using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class RcloneTransferringFile
    {
        [JsonPropertyName("bytes")]
        public long Bytes { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public long Size { get; set; }
    }
}