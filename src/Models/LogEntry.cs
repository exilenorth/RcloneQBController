using System;

namespace RcloneQBController.Models
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Script { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}