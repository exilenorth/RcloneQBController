using RcloneQBController.Models;
using RcloneQBController.Services;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public AppConfig ConfigCopy { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddJobCommand { get; }
        public ICommand EditJobCommand { get; }
        public ICommand RemoveJobCommand { get; }

        public Action<bool> CloseAction { get; set; }

        public SettingsViewModel()
        {
            // Create a deep copy of the configuration object by serializing and deserializing it.
            // This prevents changes in the settings window from affecting the main application state
            // until the user explicitly saves them.
            var originalConfig = ConfigurationService.Instance.LoadConfig();
            var json = JsonSerializer.Serialize(originalConfig);
            ConfigCopy = JsonSerializer.Deserialize<AppConfig>(json);

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save(object obj)
        {
            ConfigurationService.Instance.SaveConfig(ConfigCopy);
            CloseAction?.Invoke(true);
        }

        private bool CanSave(object obj)
        {
            return string.IsNullOrEmpty(Error);
        }

        private void Cancel(object obj)
        {
            CloseAction?.Invoke(false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Error
        {
            get
            {
                // Check all properties for errors
                if (this[nameof(ConfigCopy.Rclone.RclonePath)] != null) return "Invalid Rclone Path";
                // Add other validation checks here
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == nameof(ConfigCopy.Rclone.RclonePath))
                {
                    if (string.IsNullOrEmpty(ConfigCopy.Rclone.RclonePath) || !System.IO.File.Exists(ConfigCopy.Rclone.RclonePath))
                    {
                        result = "Rclone path is not valid.";
                    }
                }
                // Add other validation logic here
                return result;
            }
        }
    }
}