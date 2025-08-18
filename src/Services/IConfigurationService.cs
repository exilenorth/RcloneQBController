using RcloneQBController.Models;

namespace RcloneQBController.Services
{
    public interface IConfigurationService
    {
        AppConfig LoadConfig(string? configPath = null);
        void SaveConfig(AppConfig config, string? configPath = null);
        bool IsValid(AppConfig config);
        void ValidateConfiguration(AppConfig config);
        string ConfigFilePath { get; }
    }
}