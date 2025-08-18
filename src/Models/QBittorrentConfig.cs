using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class QBittorrentConfig
    {
        [JsonPropertyName("protocol")]
        public string? Protocol { get; set; }

        [JsonPropertyName("host")]
        public string? Host { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("base_path")]
        public string? BasePath { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}