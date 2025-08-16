using System.Collections.ObjectModel;
using RcloneQBController.Models;

namespace RcloneQBController.ViewModels
{
    public class ActivityDashboardViewModel
    {
        public ObservableCollection<LogEntry> LogEntries { get; } = new ObservableCollection<LogEntry>();
        public ObservableCollection<FileTransfer> FileTransfers { get; } = new ObservableCollection<FileTransfer>();

        public void ParseOutput(string output)
        {
            if (!string.IsNullOrEmpty(output))
            {
                LogEntries.Add(new LogEntry { Timestamp = DateTime.Now, Message = output });
            }
        }
    }
}