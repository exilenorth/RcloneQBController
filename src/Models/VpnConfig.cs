using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class VpnConfig
    {
        [JsonPropertyName("config_file")]
        public string ConfigFile { get; set; }

        [JsonPropertyName("client_ip")]
        public string ClientIp { get; set; }
    }
}