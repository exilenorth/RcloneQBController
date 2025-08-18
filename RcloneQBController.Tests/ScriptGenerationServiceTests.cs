using RcloneQBController.Services;
using RcloneQBController.Models;
using Moq;

namespace RcloneQBController.Tests
{
    [TestClass]
    public class ScriptGenerationServiceTests
    {
        private string _testScriptPath;

        [TestInitialize]
        public void Setup()
        {
            _testScriptPath = Path.Combine(Path.GetTempPath(), "scripts");
            if (!Directory.Exists(_testScriptPath))
            {
                Directory.CreateDirectory(_testScriptPath);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(_testScriptPath))
            {
                Directory.Delete(_testScriptPath, true);
            }
        }

        [TestMethod]
        public void GenerateScripts_WithCompleteConfig_CreatesFiles()
        {
            var config = new AppConfig
            {
                Rclone = new RcloneConfig { RemoteName = "Test" },
                QBittorrent = new QBittorrentConfig { Host = "localhost", Port = 8080 }
            };
            var mockConfigService = new Mock<IConfigurationService>();
            mockConfigService.Setup(s => s.LoadConfig(null)).Returns(config);
            var mockCredentialService = new Mock<ICredentialService>();
            var service = new ScriptGenerationService(mockConfigService.Object, mockCredentialService.Object, "script_templates");
            service.GenerateScripts(config, _testScriptPath);

            Assert.IsTrue(Directory.Exists(_testScriptPath));
            Assert.IsTrue(File.Exists(Path.Combine(_testScriptPath, "rclone_pull_media.bat")));
            Assert.IsTrue(File.Exists(Path.Combine(_testScriptPath, "qb_cleanup_ratio.ps1")));
        }

        [TestMethod]
        public void GenerateScripts_WithNullQBittorrentConfig_HandlesGracefully()
        {
            var config = new AppConfig
            {
                Rclone = new RcloneConfig { RemoteName = "Test" },
                QBittorrent = null
            };
            var mockConfigService = new Mock<IConfigurationService>();
            mockConfigService.Setup(s => s.LoadConfig(null)).Returns(config);
            var mockCredentialService = new Mock<ICredentialService>();
            var service = new ScriptGenerationService(mockConfigService.Object, mockCredentialService.Object, "script_templates");
            service.GenerateScripts(config, _testScriptPath);

            Assert.IsTrue(Directory.Exists(_testScriptPath));
            Assert.IsTrue(File.Exists(Path.Combine(_testScriptPath, "rclone_pull_media.bat")));
            Assert.IsFalse(File.Exists(Path.Combine(_testScriptPath, "qb_cleanup_ratio.ps1")));
        }
    }
}