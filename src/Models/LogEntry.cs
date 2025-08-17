using System;

namespace RcloneQBController.Models
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Success
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Script { get; set; } = string.Empty;
        public LogLevel Level { get; set; }
        public string Status => Level switch
        {
            LogLevel.Info => "ℹ️",
            LogLevel.Warning => "⚠️",
            LogLevel.Error => "❌",
            LogLevel.Success => "✅",
            _ => " "
        };
        public string Message { get; set; } = string.Empty;
    }
}