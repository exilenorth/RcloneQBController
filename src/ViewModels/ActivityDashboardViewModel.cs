using System.Collections.ObjectModel;
using RcloneQBController.Models;
using System.Text.Json;
using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;

namespace RcloneQBController.ViewModels
{
    public class ActivityDashboardViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<LogEntry> _logEntries = new ObservableCollection<LogEntry>();
        public ICollectionView LogEntries { get; }

        public ObservableCollection<FileTransfer> FileTransfers { get; } = new ObservableCollection<FileTransfer>();

        private string _logFilterText;
        public string LogFilterText
        {
            get => _logFilterText;
            set
            {
                _logFilterText = value;
                OnPropertyChanged(nameof(LogFilterText));
                LogEntries.Refresh();
            }
        }

        public ActivityDashboardViewModel()
        {
            LogEntries = CollectionViewSource.GetDefaultView(_logEntries);
            LogEntries.Filter = FilterLogEntries;
        }

        private bool FilterLogEntries(object item)
        {
            if (string.IsNullOrEmpty(LogFilterText))
            {
                return true;
            }

            if (item is LogEntry logEntry)
            {
                return logEntry.Message.IndexOf(LogFilterText, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }


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
                    App.Current.Dispatcher.Invoke(() =>
                    _logEntries.Add(new LogEntry { Timestamp = DateTime.Now, Message = rcloneLog.Message, Level = ParseLogLevel(rcloneLog.Level) }));
                }
            }
            catch (JsonException)
            {
                 App.Current.Dispatcher.Invoke(() =>
                _logEntries.Add(new LogEntry { Timestamp = DateTime.Now, Message = output, Level = ParseLogLevel(output) }));
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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}