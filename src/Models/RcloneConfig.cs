using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class RcloneConfig
    {
        [JsonPropertyName("rclone_path")]
        public string RclonePath { get; set; }

        [JsonPropertyName("remote_name")]
        public string RemoteName { get; set; }

        [JsonPropertyName("log_dir")]
        public string LogDir { get; set; }

        [JsonPropertyName("use_json_log")]
        public bool UseJsonLog { get; set; }

        [JsonPropertyName("flags")]
        public Dictionary<string, object> Flags { get; set; }

        [JsonPropertyName("jobs")]
        public List<RcloneJobConfig> Jobs { get; set; }
    }
}