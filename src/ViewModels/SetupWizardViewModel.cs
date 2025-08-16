using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RcloneQBController.ViewModels
{
    public class SetupWizardViewModel : INotifyPropertyChanged
    {
        private object _currentStepViewModel;
        private List<object> _steps;
        private int _currentStepIndex;

        public object CurrentStepViewModel
        {
            get => _currentStepViewModel;
            set
            {
                _currentStepViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand NextCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand FinishCommand { get; }

        public SetupWizardViewModel()
        {
            _steps = new List<object>
            {
                new RcloneInstallViewModel(),
                new SeedboxConnectionViewModel(),
                new TransferJobViewModel(),
                new RcloneSummaryViewModel(),
                new OpenVPNViewModel(),
                new QbittorrentViewModel(),
                new CleanupSummaryViewModel()
            };

            _currentStepIndex = 0;
            CurrentStepViewModel = _steps[_currentStepIndex];

            NextCommand = new RelayCommand(GoToNextStep, CanGoToNextStep);
            BackCommand = new RelayCommand(GoToPreviousStep, CanGoToPreviousStep);
            FinishCommand = new RelayCommand(FinishWizard);
        }

        private void GoToNextStep(object parameter)
        {
            if (CanGoToNextStep(parameter))
            {
                _currentStepIndex++;
                CurrentStepViewModel = _steps[_currentStepIndex];
            }
        }

        private bool CanGoToNextStep(object parameter) => _currentStepIndex < _steps.Count - 1;

        private void GoToPreviousStep(object parameter)
        {
            if (CanGoToPreviousStep(parameter))
            {
                _currentStepIndex--;
                CurrentStepViewModel = _steps[_currentStepIndex];
            }
        }

        private bool CanGoToPreviousStep(object parameter) => _currentStepIndex > 0;

        private void FinishWizard(object parameter)
        {
            // Logic to save configuration and close the wizard
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}