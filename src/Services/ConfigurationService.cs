using RcloneQBController.Models;
using System;
using System.IO;
using System.Text.Json;

namespace RcloneQBController.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private const string ConfigFileName = "config.json";

        public string ConfigFilePath => ConfigFileName;

        public AppConfig LoadConfig(string? configPath = null)
        {
            var path = configPath ?? ConfigFilePath;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Configuration file not found.", path);
            }

            var json = File.ReadAllText(path);
            var config = JsonSerializer.Deserialize<AppConfig>(json);
            if (config == null)
            {
                throw new InvalidDataException("Failed to deserialize configuration file.");
            }
            PurgeOldLogs(config);
            return config;
        }

        public void SaveConfig(AppConfig config, string? configPath = null)
        {
            var path = configPath ?? ConfigFileName;
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(path, json);
        }
        public bool IsValid(AppConfig config)
        {
            if (config == null) return false;

            // Rclone validation
            if (config.Rclone == null ||
                string.IsNullOrWhiteSpace(config.Rclone.RclonePath) ||
                string.IsNullOrWhiteSpace(config.Rclone.RemoteName))
            {
                return false;
            }

            // QBittorrent validation
            if (config.QBittorrent == null ||
                string.IsNullOrWhiteSpace(config.QBittorrent.Host) ||
                config.QBittorrent.Port <= 0)
            {
                return false;
            }

            return true;
        }

        public void ValidateConfiguration(AppConfig config)
        {
            // Path Validation
            if (config.Rclone?.RclonePath == null || !File.Exists(config.Rclone.RclonePath)) throw new FileNotFoundException("rclone.exe not found.", config.Rclone?.RclonePath);
            if (config.Rclone?.LogDir != null && !Directory.Exists(config.Rclone.LogDir)) Directory.CreateDirectory(config.Rclone.LogDir);
            if (config.Rclone?.Jobs != null)
            {
                foreach (var job in config.Rclone.Jobs)
                {
                    if (job.DestPath != null && !Directory.Exists(job.DestPath)) throw new DirectoryNotFoundException($"Destination path for job '{job.Name}' not found.");
                }
            }
            if (config.Vpn?.ConfigFile == null || !File.Exists(config.Vpn.ConfigFile)) throw new FileNotFoundException("VPN config file not found.", config.Vpn?.ConfigFile);

            // Network Validation
            if (config.QBittorrent != null && (config.QBittorrent.Port < 1 || config.QBittorrent.Port > 65535)) throw new ArgumentOutOfRangeException("Invalid qBittorrent port.");

            // Value Validation
            if (config.Cleanup != null && config.Cleanup.TargetRatio < 0) throw new ArgumentOutOfRangeException("Target ratio must be non-negative.");
            if (config.Schedule != null && config.Schedule.PullEveryMinutes <= 0) throw new ArgumentOutOfRangeException("Pull interval must be positive.");
            if (config.AppSettings != null && config.AppSettings.LogRetentionDays <= 0) throw new ArgumentOutOfRangeException("Log retention days must be positive.");
            if (config.Rclone?.Jobs != null)
            {
                foreach (var job in config.Rclone.Jobs)
                {
                    if (job.MaxRuntimeMinutes <= 0) throw new ArgumentOutOfRangeException($"Max runtime for job '{job.Name}' must be positive.");
                }
            }
        }

        private void PurgeOldLogs(AppConfig config)
        {
            if (config?.AppSettings == null) return;

            void Purge(string? dir)
            {
                if (string.IsNullOrWhiteSpace(dir)) return;
                var d = new DirectoryInfo(dir);
                if (!d.Exists) return;
                foreach (var f in d.GetFiles("*.log"))
                    if (f.LastWriteTime < DateTime.Now.AddDays(-config.AppSettings.LogRetentionDays))
                        f.Delete();
            }

            Purge(config.Rclone?.LogDir);
            Purge(config.Cleanup?.LogDir); // NEW: also cleanup logs
        }
    }
}