using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class AppConfig
    {
        [JsonPropertyName("app_settings")]
        public AppSettings AppSettings { get; set; }

        [JsonPropertyName("rclone")]
        public RcloneConfig Rclone { get; set; }

        [JsonPropertyName("seedbox")]
        public SeedboxConfig Seedbox { get; set; }

        [JsonPropertyName("vpn")]
        public VpnConfig Vpn { get; set; }

        [JsonPropertyName("qbittorrent")]
        public QBittorrentConfig QBittorrent { get; set; }

        [JsonPropertyName("cleanup")]
        public CleanupScriptConfig Cleanup { get; set; }

        [JsonPropertyName("schedule")]
        public SchedulerConfig Schedule { get; set; }
    }

    public class AppSettings
    {
        [JsonPropertyName("log_retention_days")]
        public int LogRetentionDays { get; set; }
    }
}