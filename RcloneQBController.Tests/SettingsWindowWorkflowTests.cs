using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RcloneQBController.Services;
using RcloneQBController.ViewModels;
using RcloneQBController.Models;
using System.IO;
using Newtonsoft.Json;

namespace RcloneQBController.Tests
{
    [TestClass]
    public class SettingsWindowWorkflowTests
    {
        private Mock<IConfigurationService> _mockConfigService;
        private Mock<ICredentialService> _mockCredentialService;
        private Mock<IUserNotifierService> _mockNotifierService;
        private SettingsViewModel _viewModel;
        private string _testConfigPath = "test_config.json";

        [TestInitialize]
        public void TestInitialize()
        {
            _mockConfigService = new Mock<IConfigurationService>();
            _mockCredentialService = new Mock<ICredentialService>();
            _mockNotifierService = new Mock<IUserNotifierService>();

            var appConfig = new AppConfig
            {
                AppSettings = new AppSettings { LogRetentionDays = 10 },
                QBittorrent = new QBittorrentConfig()
            };

            _mockConfigService.Setup(s => s.LoadConfig(null)).Returns(appConfig);
            _mockConfigService.Setup(s => s.ConfigFilePath).Returns(_testConfigPath);

            _viewModel = new SettingsViewModel(_mockConfigService.Object, _mockCredentialService.Object, _mockNotifierService.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(_testConfigPath))
            {
                File.Delete(_testConfigPath);
            }
        }
        [TestMethod]
        public void Settings_Cancel_DoesNotSaveChanges()
        {
            // Arrange
            var initialConfig = new AppConfig
            {
                AppSettings = new AppSettings { LogRetentionDays = 10 }
            };
            File.WriteAllText(_testConfigPath, JsonConvert.SerializeObject(initialConfig));
            _viewModel.ConfigCopy.AppSettings.LogRetentionDays = 20;

            // Act
            _viewModel.CancelCommand.Execute(null);

            // Assert
            var configOnDisk = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(_testConfigPath));
            Assert.AreEqual(10, configOnDisk.AppSettings.LogRetentionDays);
        }

        [TestMethod]
        public void Settings_Save_PersistsChanges()
        {
            // Arrange
            _viewModel.ConfigCopy.AppSettings.LogRetentionDays = 20;

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockConfigService.Verify(s => s.SaveConfig(_viewModel.ConfigCopy, null), Times.Once);
        }

        [TestMethod]
        public void Settings_Save_UpdatesCredential()
        {
            // Arrange
            _viewModel.ConfigCopy.QBittorrent.Username = "testuser";
            _viewModel.QBittorrentPassword.Clear();
            "newpassword".ToList().ForEach(_viewModel.QBittorrentPassword.AppendChar);

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockCredentialService.Verify(s => s.SaveCredential("RcloneQBController_qBittorrent", "testuser", "newpassword"), Times.Once);
        }
    }
}