using RcloneQBController.Services;
using RcloneQBController.Models;
using System.Text.Json;

namespace RcloneQBController.Tests
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        private const string ValidConfig = @"{ ""rclone"": { ""remote_name"": ""Test"", ""rclone_path"": ""C:\\rclone.exe"" }, ""qbittorrent"": { ""host"": ""localhost"", ""port"": 8080 } }";
        private const string CorruptConfig = @"{ ""rclone"": { ""remote_name"": ""Test"" }, ""qbittorrent"": { ""host"": ""localhost""";
        private string _testConfigPath;

        [TestInitialize]
        public void Setup()
        {
            _testConfigPath = Path.GetTempFileName();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testConfigPath))
            {
                File.Delete(_testConfigPath);
            }
        }

        [TestMethod]
        public void LoadConfig_ValidFile_ReturnsConfig()
        {
            var service = new ConfigurationService();
            service.SaveConfig(JsonSerializer.Deserialize<AppConfig>(ValidConfig), _testConfigPath);
            var config = service.LoadConfig(_testConfigPath);
            Assert.IsNotNull(config);
            Assert.AreEqual("Test", config.Rclone.RemoteName);
        }

        [TestMethod]
        public void LoadConfig_MissingFile_ThrowsFileNotFoundException()
        {
            if (File.Exists(_testConfigPath))
            {
                File.Delete(_testConfigPath);
            }
            var service = new ConfigurationService();
            Assert.ThrowsException<FileNotFoundException>(() => service.LoadConfig(_testConfigPath));
        }

        [TestMethod]
        public void LoadConfig_CorruptFile_ThrowsJsonException()
        {
            File.WriteAllText(_testConfigPath, CorruptConfig);
            var service = new ConfigurationService();
            Assert.ThrowsException<JsonException>(() => service.LoadConfig(_testConfigPath));
        }

        [TestMethod]
        public void IsValid_WithValidConfig_ReturnsTrue()
        {
            var config = new AppConfig
            {
                Rclone = new RcloneConfig { RemoteName = "Test", RclonePath = "C:\\rclone.exe" },
                QBittorrent = new QBittorrentConfig { Host = "localhost", Port = 8080 }
            };
            var service = new ConfigurationService();
            Assert.IsTrue(service.IsValid(config));
        }

        [TestMethod]
        public void IsValid_WithInvalidConfig_ReturnsFalse()
        {
            var config = new AppConfig
            {
                Rclone = new RcloneConfig { RemoteName = "" },
                QBittorrent = new QBittorrentConfig { Host = "localhost", Port = 8080 }
            };
            var service = new ConfigurationService();
            Assert.IsFalse(service.IsValid(config));
        }
    }
}