using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class SchedulerConfig
    {
        [JsonPropertyName("pull_every_minutes")]
        public int PullEveryMinutes { get; set; }

        [JsonPropertyName("cleanup_offset_minutes")]
        public int CleanupOffsetMinutes { get; set; }

        [JsonPropertyName("only_when_logged_in")]
        public bool OnlyWhenLoggedIn { get; set; }
    }
}