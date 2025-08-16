using RcloneQBController.Models;
using System;
using System.IO;
using System.Text.Json;

namespace RcloneQBController.Services
{
    public class ConfigurationService
    {
        private static readonly Lazy<ConfigurationService> _instance = new Lazy<ConfigurationService>(() => new ConfigurationService());
        private const string ConfigFileName = "config.json";

        public static ConfigurationService Instance => _instance.Value;

        private ConfigurationService() { }

        public AppConfig LoadConfig()
        {
            if (!File.Exists(ConfigFileName))
            {
                return null;
            }

            var json = File.ReadAllText(ConfigFileName);
            var config = JsonSerializer.Deserialize<AppConfig>(json);
            PurgeOldLogs(config);
            return config;
        }

        public void SaveConfig(AppConfig config)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(ConfigFileName, json);
        }

        public void ValidateConfiguration(AppConfig config)
        {
            // Path Validation
            if (!File.Exists(config.Rclone.RclonePath)) throw new FileNotFoundException("rclone.exe not found.", config.Rclone.RclonePath);
            if (!Directory.Exists(config.Rclone.LogDir)) Directory.CreateDirectory(config.Rclone.LogDir);
            foreach (var job in config.Rclone.Jobs)
            {
                if (!Directory.Exists(job.DestPath)) throw new DirectoryNotFoundException($"Destination path for job '{job.Name}' not found.");
            }
            if (!File.Exists(config.Vpn.ConfigFile)) throw new FileNotFoundException("VPN config file not found.", config.Vpn.ConfigFile);

            // Network Validation
            if (config.QBittorrent.Port < 1 || config.QBittorrent.Port > 65535) throw new ArgumentOutOfRangeException("Invalid qBittorrent port.");

            // Value Validation
            if (config.Cleanup.TargetRatio < 0) throw new ArgumentOutOfRangeException("Target ratio must be non-negative.");
            if (config.Schedule.PullEveryMinutes <= 0) throw new ArgumentOutOfRangeException("Pull interval must be positive.");
            if (config.AppSettings.LogRetentionDays <= 0) throw new ArgumentOutOfRangeException("Log retention days must be positive.");
            foreach (var job in config.Rclone.Jobs)
            {
                if (job.MaxRuntimeMinutes <= 0) throw new ArgumentOutOfRangeException($"Max runtime for job '{job.Name}' must be positive.");
            }
        }

        private void PurgeOldLogs(AppConfig config)
        {
            if (config?.AppSettings == null || config.Rclone?.LogDir == null) return;

            var logDir = new DirectoryInfo(config.Rclone.LogDir);
            if (!logDir.Exists) return;

            foreach (var file in logDir.GetFiles("*.log"))
            {
                if (file.LastWriteTime < DateTime.Now.AddDays(-config.AppSettings.LogRetentionDays))
                {
                    file.Delete();
                }
            }
        }
    }
}