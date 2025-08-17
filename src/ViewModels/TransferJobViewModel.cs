using RcloneQBController.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class TransferJobViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<RcloneJobConfig>? Jobs { get; set; }
        public ICommand AddJobCommand { get; }
        public ICommand EditJobCommand { get; }
        public ICommand RemoveJobCommand { get; }

        public TransferJobViewModel(string? defaultSourcePath = null)
        {
            var tvSourcePath = !string.IsNullOrEmpty(defaultSourcePath) ? $"{defaultSourcePath}/Media/TV" : "/home/user/torrents/qbittorrent/Media/TV";
            var moviesSourcePath = !string.IsNullOrEmpty(defaultSourcePath) ? $"{defaultSourcePath}/Media/Movies" : "/home/user/torrents/qbittorrent/Media/Movies";

            Jobs = new ObservableCollection<RcloneJobConfig>
            {
                new RcloneJobConfig { Name = "TV Shows", SourcePath = tvSourcePath, DestPath = "D:\\Media\\TV" },
                new RcloneJobConfig { Name = "Movies", SourcePath = moviesSourcePath, DestPath = "D:\\Media\\Movies" }
            };

            AddJobCommand = new RelayCommand(AddJob);
            EditJobCommand = new RelayCommand(EditJob, CanEditOrRemoveJob);
            RemoveJobCommand = new RelayCommand(RemoveJob, CanEditOrRemoveJob);
        }

        private void AddJob(object parameter)
        {
            // Logic to open a dialog and add a new job
        }

        private void EditJob(object parameter)
        {
            if (parameter is RcloneJobConfig job)
            {
                // Logic to open a dialog and edit the selected job
            }
        }

        private void RemoveJob(object parameter)
        {
            if (parameter is RcloneJobConfig job)
            {
                Jobs?.Remove(job);
            }
        }

        private bool CanEditOrRemoveJob(object parameter) => parameter is RcloneJobConfig;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}