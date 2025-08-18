using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RcloneQBController.Services;
using RcloneQBController.ViewModels;
using RcloneQBController.Models;

namespace RcloneQBController.Tests
{
    [TestClass]
    public class ScriptExecutionWorkflowTests
    {
        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<ICredentialService> _mockCredentialService;
        private Mock<IScriptGenerationService> _mockScriptGenerationService;
        private Mock<IScriptRunnerService> _mockScriptRunnerService;
        private Mock<IUserNotifierService> _mockUserNotifierService;
        private Mock<INotificationService> _mockNotificationService;
        private MainViewModel _mainViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockCredentialService = new Mock<ICredentialService>();
            _mockScriptGenerationService = new Mock<IScriptGenerationService>();
            _mockScriptRunnerService = new Mock<IScriptRunnerService>();
            _mockUserNotifierService = new Mock<IUserNotifierService>();
            _mockNotificationService = new Mock<INotificationService>();

            var appConfig = new AppConfig
            {
                Rclone = new RcloneConfig
                {
                    Jobs = new System.Collections.Generic.List<RcloneJobConfig>
                    {
                        new RcloneJobConfig { Name = "TestJob", MaxRuntimeMinutes = 5 }
                    }
                }
            };

            _mockConfigurationService.Setup(s => s.LoadConfig(null)).Returns(appConfig);

            _mainViewModel = new MainViewModel(
                _mockConfigurationService.Object,
                _mockCredentialService.Object,
                _mockScriptGenerationService.Object,
                _mockScriptRunnerService.Object,
                _mockUserNotifierService.Object,
                _mockNotificationService.Object
            );
        }

        [TestMethod]
        public async Task RunNow_ExecutesJob_And_Notifies()
        {
            // Arrange
            var job = _mainViewModel.Jobs[0];

            // Act
            ((RelayCommand)_mainViewModel.RunJobCommand).Execute(job);

            // Assert
            _mockScriptRunnerService.Verify(s => s.RunRcloneJobAsync(job, It.IsAny<Action<string>>()), Times.Once);
            _mockNotificationService.Verify(s => s.ShowNotification(It.IsAny<string>(), It.Is<string>(m => m.Contains("completed successfully"))), Times.Once);
        }

        [TestMethod]
        public async Task RunNow_WithTimeout_TerminatesAndNotifies()
        {
            // Arrange
            var job = new RcloneJobConfig { Name = "TimeoutJob", MaxRuntimeMinutes = 1 };
            _mainViewModel.Jobs.Add(job);
            _mockScriptRunnerService.Setup(s => s.RunRcloneJobAsync(job, It.IsAny<Action<string>>()))
                .Callback<RcloneJobConfig, Action<string>>((j, a) =>
                {
                    a("ERROR: Process timed out after 1 minutes and was terminated.");
                });


            // Act
            ((RelayCommand)_mainViewModel.RunJobCommand).Execute(job);

            // Assert
            _mockScriptRunnerService.Verify(s => s.RunRcloneJobAsync(job, It.IsAny<Action<string>>()), Times.Once);
            _mockNotificationService.Verify(s => s.ShowNotification(It.IsAny<string>(), It.Is<string>(m => m.Contains("timed out"))), Times.Once);
        }
    }
}