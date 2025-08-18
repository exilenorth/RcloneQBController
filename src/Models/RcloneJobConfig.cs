using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace RcloneQBController.Models
{
    public class RcloneJobConfig : INotifyPropertyChanged
    {
        private bool _isRunning;
        private bool _isScheduled;

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("source_path")]
        public string? SourcePath { get; set; }

        [JsonPropertyName("dest_path")]
        public string? DestPath { get; set; }

        [JsonPropertyName("max_runtime_minutes")]
        public int MaxRuntimeMinutes { get; set; }

        public bool IsScheduled
        {
            get => _isScheduled;
            set
            {
                if (_isScheduled != value)
                {
                    _isScheduled = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}