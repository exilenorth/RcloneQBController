using RcloneQBController.Models;

namespace RcloneQBController.Services
{
    public interface IScriptGenerationService
    {
        string GetPreviewCommand(RcloneJobConfig job);
        void GenerateScripts(AppConfig config, string? outputDirectory = null);
    }
}