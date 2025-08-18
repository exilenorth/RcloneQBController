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
            if (job == null) throw new System.ArgumentNullException(nameof(job));

            var config = _configurationService.LoadConfig();
            if (config.Rclone == null) throw new System.ArgumentNullException(nameof(config.Rclone));

            var templateDirectory = Path.Combine(System.AppContext.BaseDirectory, "script_templates");
            var rcloneTemplatePath = Path.Combine(templateDirectory, "rclone_pull_media.bat.template");

            if (File.Exists(rcloneTemplatePath))
            {
                var rcloneTemplate = File.ReadAllText(rcloneTemplatePath);
                var scriptContent = rcloneTemplate
                    .Replace("%%RCLONE_EXE_PATH%%", Sanitize(config.Rclone.RclonePath ?? string.Empty))
                    .Replace("%%SOURCE%%", Sanitize(job.SourcePath ?? string.Empty))
                    .Replace("%%DEST%%", Sanitize(job.DestPath ?? string.Empty))
                    .Replace("%%LOG_DIR%%", Sanitize(config.Rclone.LogDir ?? string.Empty))
                    .Replace("%%FILTER_FILE%%", Sanitize(job.FilterFile ?? string.Empty))
                    .Replace("%%LOG_LEVEL%%", Sanitize(config.Rclone.LogLevel ?? "INFO"))
                    .Replace("%%MIN_AGE%%", Sanitize(config.Rclone.Flags?.MinAge ?? string.Empty))
                    .Replace("%%TRANSFERS%%", config.Rclone.Flags?.Transfers.ToString() ?? "4")
                    .Replace("%%CHECKERS%%", config.Rclone.Flags?.Checkers.ToString() ?? "8");

                return scriptContent;
            }

            throw new FileNotFoundException($"Template file not found at: {rcloneTemplatePath}");
        }

        public void GenerateScripts(AppConfig config)
        {
            var templateDirectory = Path.Combine(System.AppContext.BaseDirectory, "script_templates");
            var outputDirectory = Path.Combine(System.AppContext.BaseDirectory, "scripts");

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
                        var scriptContent = rcloneTemplate
                            .Replace("%%RCLONE_EXE_PATH%%", Sanitize(config.Rclone.RclonePath ?? string.Empty))
                            .Replace("%%SOURCE%%", Sanitize(job.SourcePath ?? string.Empty))
                            .Replace("%%DEST%%", Sanitize(job.DestPath ?? string.Empty))
                            .Replace("%%LOG_DIR%%", Sanitize(config.Rclone.LogDir ?? string.Empty))
                            .Replace("%%FILTER_FILE%%", Sanitize(job.FilterFile ?? string.Empty))
                            .Replace("%%LOG_LEVEL%%", Sanitize(config.Rclone.LogLevel ?? "INFO"))
                            .Replace("%%MIN_AGE%%", Sanitize(config.Rclone.Flags?.MinAge ?? string.Empty))
                            .Replace("%%TRANSFERS%%", config.Rclone.Flags?.Transfers.ToString() ?? "4")
                            .Replace("%%CHECKERS%%", config.Rclone.Flags?.Checkers.ToString() ?? "8");

                        var sanitizedJobName = "default_job";
                        if (!string.IsNullOrEmpty(job.Name))
                        {
                            sanitizedJobName = Sanitize(job.Name);
                        }
                        var outputFileName = $"rclone_pull_{sanitizedJobName}.bat";
                        var outputPath = Path.Combine(outputDirectory, outputFileName);

                        scriptContent = scriptContent.Replace("rclone_pull.lock", $"rclone_pull_{sanitizedJobName}.lock");

                        if (scriptContent.Contains("%%"))
                            throw new System.InvalidOperationException("Template has unresolved placeholders.");
                        File.WriteAllText(outputPath, scriptContent);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Rclone template file not found at: {rcloneTemplatePath}");
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
            else
            {
                throw new FileNotFoundException($"qBittorrent cleanup template file not found at: {qbTemplatePath}");
            }
        }

        private static string Sanitize(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var safe = new string(name.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
            return safe.Replace(' ', '_').ToLowerInvariant();
        }
    }
}