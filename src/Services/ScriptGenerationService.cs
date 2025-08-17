using RcloneQBController.Models;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace RcloneQBController.Services
{
    public class ScriptGenerationService
    {
        private readonly ConfigurationService _configurationService;

        public ScriptGenerationService(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public string GetPreviewCommand(RcloneJobConfig job)
        {
            var config = _configurationService.LoadConfig();
            var templateDirectory = Path.Combine(Directory.GetCurrentDirectory(), "script_templates");
            var rcloneTemplatePath = Path.Combine(templateDirectory, "rclone_pull_media.bat.template");

            if (File.Exists(rcloneTemplatePath))
            {
                var rcloneTemplate = File.ReadAllText(rcloneTemplatePath);
                var scriptContent = new StringBuilder(rcloneTemplate);

                // --- Replace General Rclone Settings ---
                scriptContent.Replace("%%RCLONE_EXE_PATH%%", config.Rclone.RclonePath);
                scriptContent.Replace("%%LOG_DIR%%", config.Rclone.LogDir);
                if (config.Rclone.Flags != null)
                {
                    foreach (var flag in config.Rclone.Flags)
                    {
                        scriptContent.Replace($"%%{flag.Key.ToUpper()}%%", flag.Value.ToString());
                    }
                }

                // --- Replace Job-Specific Settings ---
                var sourceRemotePath = $"{config.Rclone.RemoteName}:{job.SourcePath}";
                scriptContent.Replace("%%SOURCE_REMOTE%%", sourceRemotePath);
                scriptContent.Replace("%%DEST_PATH%%", job.DestPath);

                return scriptContent.ToString();
            }

            return "Template file not found.";
        }

        public void GenerateScripts(AppConfig config)
        {
            var templateDirectory = Path.Combine(Directory.GetCurrentDirectory(), "script_templates");
            var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "scripts");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // --- Generate Rclone Scripts ---
            if (config.Rclone?.Jobs != null && config.Rclone.Jobs.Any())
            {
                var rcloneTemplatePath = Path.Combine(templateDirectory, "rclone_pull_media.bat.template");
                if (File.Exists(rcloneTemplatePath))
                {
                    var rcloneTemplate = File.ReadAllText(rcloneTemplatePath);

                    foreach (var job in config.Rclone.Jobs)
                    {
                        var scriptContent = new StringBuilder(rcloneTemplate);

                        // --- Replace General Rclone Settings ---
                        scriptContent.Replace("%%RCLONE_EXE_PATH%%", config.Rclone.RclonePath);
                        scriptContent.Replace("%%LOG_DIR%%", config.Rclone.LogDir);
                        if (config.Rclone.Flags != null)
                        {
                            foreach (var flag in config.Rclone.Flags)
                            {
                                scriptContent.Replace($"%%{flag.Key.ToUpper()}%%", flag.Value.ToString());
                            }
                        }

                        // --- Replace Job-Specific Settings ---
                        var sourceRemotePath = $"{config.Rclone.RemoteName}:{job.SourcePath}";
                        scriptContent.Replace("%%SOURCE_REMOTE%%", sourceRemotePath);
                        scriptContent.Replace("%%DEST_PATH%%", job.DestPath);

                        var outputFileName = $"rclone_pull_{job.Name}.bat";
                        var outputPath = Path.Combine(outputDirectory, outputFileName);

                        File.WriteAllText(outputPath, scriptContent.ToString());
                    }
                }
            }

            // --- Generate qB Cleanup Script ---
            var qbTemplatePath = Path.Combine(templateDirectory, "qb_cleanup_ratio.ps1.template");
            if (File.Exists(qbTemplatePath))
            {
                var qbTemplate = File.ReadAllText(qbTemplatePath);
                var qbOutputPath = Path.Combine(outputDirectory, "qb_cleanup_ratio.ps1");
                File.WriteAllText(qbOutputPath, qbTemplate);
            }
        }
    }
}