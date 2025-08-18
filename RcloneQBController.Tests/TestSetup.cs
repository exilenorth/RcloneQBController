using Moq;
using RcloneQBController.Models;
using RcloneQBController.Services;
using RcloneQBController.ViewModels;

namespace RcloneQBController.Tests
{
    public class TestSetup
    {
        public Mock<IConfigurationService> MockConfigService { get; }
        public Mock<ICredentialService> MockCredentialService { get; }
        public Mock<IUserNotifierService> MockNotifierService { get; }
        public Mock<IScriptGenerationService> MockScriptGenerationService { get; }
        public Mock<IScriptRunnerService> MockScriptRunnerService { get; }
        public Mock<INotificationService> MockNotificationService { get; }
        public MainViewModel MainViewModel { get; }
        public AppConfig TestConfig { get; }

        public TestSetup()
        {
            MockConfigService = new Mock<IConfigurationService>();
            MockCredentialService = new Mock<ICredentialService>();
            MockNotifierService = new Mock<IUserNotifierService>();
            MockScriptGenerationService = new Mock<IScriptGenerationService>();
            MockScriptRunnerService = new Mock<IScriptRunnerService>();
            MockNotificationService = new Mock<INotificationService>();

            TestConfig = new AppConfig
            {
                QBittorrent = new QBittorrentConfig { Username = "testuser" },
                Rclone = new RcloneConfig()
            };

            MockConfigService.Setup(s => s.LoadConfig(It.IsAny<string>())).Returns(TestConfig);

            MainViewModel = new MainViewModel(
                MockConfigService.Object,
                MockCredentialService.Object,
                MockScriptGenerationService.Object,
                MockScriptRunnerService.Object,
                MockNotifierService.Object,
                MockNotificationService.Object
            );
        }
    }
}