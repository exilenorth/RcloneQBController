using Moq;
using RcloneQBController.Models;
using RcloneQBController.Services;
using RcloneQBController.ViewModels;

namespace RcloneQBController.Tests
{
    [TestClass]
    public class SettingsWindowWorkflowTests
    {
        private TestSetup _testSetup;

        [TestInitialize]
        public void Setup()
        {
            _testSetup = new TestSetup();
        }

        [TestMethod]
        public void Settings_Cancel_DoesNotSaveChanges()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(
                _testSetup.MockConfigService.Object,
                _testSetup.MockCredentialService.Object,
                _testSetup.MockNotifierService.Object
            );
            settingsViewModel.ConfigCopy.QBittorrent.Username = "new_user";

            // Act
            settingsViewModel.CancelCommand.Execute(null);

            // Assert
            _testSetup.MockConfigService.Verify(s => s.SaveConfig(It.IsAny<AppConfig>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Settings_Save_PersistsChanges()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(
                _testSetup.MockConfigService.Object,
                _testSetup.MockCredentialService.Object,
                _testSetup.MockNotifierService.Object
            );
            var newUsername = "updated_user";
            settingsViewModel.ConfigCopy.QBittorrent.Username = newUsername;

            // Act
            settingsViewModel.SaveCommand.Execute(null);

            // Assert
            _testSetup.MockConfigService.Verify(s => s.SaveConfig(It.Is<AppConfig>(c => c.QBittorrent.Username == newUsername), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Settings_Save_UpdatesCredential()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(
                _testSetup.MockConfigService.Object,
                _testSetup.MockCredentialService.Object,
                _testSetup.MockNotifierService.Object
            );
            var newPassword = "new_password";
            settingsViewModel.QBittorrentPassword = new System.Security.SecureString();
            newPassword.ToCharArray().ToList().ForEach(c => settingsViewModel.QBittorrentPassword.AppendChar(c));


            // Act
            settingsViewModel.SaveCommand.Execute(null);

            // Assert
            _testSetup.MockCredentialService.Verify(s => s.SaveCredential(It.IsAny<string>(), It.IsAny<string>(), newPassword), Times.Once);
        }
    }
}