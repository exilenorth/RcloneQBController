using System.Collections.ObjectModel;
using RcloneQBController.Models;
using System.Text.Json;
using System;
using System.Linq;

namespace RcloneQBController.ViewModels
{
    public class ActivityDashboardViewModel
    {
        public ObservableCollection<LogEntry> LogEntries { get; } = new ObservableCollection<LogEntry>();
        public ObservableCollection<FileTransfer> FileTransfers { get; } = new ObservableCollection<FileTransfer>();

        public void ParseOutput(string output)
        {
            if (string.IsNullOrEmpty(output)) return;

            try
            {
                var rcloneLog = JsonSerializer.Deserialize<RcloneLogEntry>(output);
                if (rcloneLog != null && rcloneLog.Stats != null)
                {
                    UpdateFileTransfers(rcloneLog);
                }
                else if (rcloneLog != null)
                {
                    LogEntries.Add(new LogEntry { Timestamp = DateTime.Now, Message = rcloneLog.Message, Level = ParseLogLevel(rcloneLog.Level) });
                }
            }
            catch (JsonException)
            {
                LogEntries.Add(new LogEntry { Timestamp = DateTime.Now, Message = output, Level = ParseLogLevel(output) });
            }
        }

        private LogLevel ParseLogLevel(string text)
        {
            if (string.IsNullOrEmpty(text)) return LogLevel.Info;
            if (text.ToLower().Contains("error")) return LogLevel.Error;
            if (text.ToLower().Contains("warn")) return LogLevel.Warning;
            if (text.ToLower().Contains("success")) return LogLevel.Success;
            return LogLevel.Info;
        }

        private void UpdateFileTransfers(RcloneLogEntry log)
        {
            // This is a simplified approach. A more robust implementation would
            // track individual files based on their names.
            if (log.Stats.Transfers > 0)
            {
                var transfer = FileTransfers.FirstOrDefault();
                if (transfer == null)
                {
                    transfer = new FileTransfer { FileName = "Overall Progress" };
                    FileTransfers.Add(transfer);
                }

                transfer.Size = log.Stats.TotalBytes;
                if (log.Stats.TotalBytes > 0)
                {
                    transfer.Progress = (double)log.Stats.Bytes / log.Stats.TotalBytes * 100;
                }
            }
        }
    }
}