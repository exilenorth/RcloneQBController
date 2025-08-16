using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class SeedboxConfig
    {
        [JsonPropertyName("host")]
        public string? Host { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("auth")]
        public AuthConfig? Auth { get; set; }
    }

    public class AuthConfig
    {
        [JsonPropertyName("method")]
        public string? Method { get; set; }

        [JsonPropertyName("pass_obscured")]
        public string? PassObscured { get; set; }
    }
}