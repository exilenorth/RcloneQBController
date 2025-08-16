using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class CleanupScriptConfig
    {
        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; }

        [JsonPropertyName("target_ratio")]
        public double TargetRatio { get; set; }

        [JsonPropertyName("hnr_minutes")]
        public int HnrMinutes { get; set; }

        [JsonPropertyName("min_age_minutes")]
        public int MinAgeMinutes { get; set; }

        [JsonPropertyName("safe_states")]
        public List<string> SafeStates { get; set; }

        [JsonPropertyName("delete_mode")]
        public string DeleteMode { get; set; }

        [JsonPropertyName("log_dir")]
        public string LogDir { get; set; }

        [JsonPropertyName("dated_logs")]
        public bool DatedLogs { get; set; }
    }
}