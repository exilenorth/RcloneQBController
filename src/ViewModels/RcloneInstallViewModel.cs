using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace RcloneQBController.ViewModels
{
    public class RcloneInstallViewModel : INotifyPropertyChanged
    {
        private bool _isRcloneInstalled;
        private string _rclonePath;

        public bool IsRcloneInstalled
        {
            get => _isRcloneInstalled;
            set
            {
                _isRcloneInstalled = value;
                OnPropertyChanged();
            }
        }

        public string RclonePath
        {
            get => _rclonePath;
            set
            {
                _rclonePath = value;
                OnPropertyChanged();
            }
        }

        public RcloneInstallViewModel()
        {
            CheckRcloneInstallation();
        }

        private void CheckRcloneInstallation()
        {
            // Logic from TECHNICAL_SPECIFICATION.md to find rclone.exe
            // 1. Check config.json (not applicable in wizard)
            // 2. Search PATH
            var pathVar = Environment.GetEnvironmentVariable("PATH");
            var paths = pathVar.Split(';');
            foreach (var path in paths)
            {
                var rclonePath = Path.Combine(path, "rclone.exe");
                if (File.Exists(rclonePath))
                {
                    IsRcloneInstalled = true;
                    RclonePath = rclonePath;
                    return;
                }
            }
            IsRcloneInstalled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}