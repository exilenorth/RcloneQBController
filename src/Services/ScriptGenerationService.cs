using RcloneQBController.Models;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace RcloneQBController.Services
{
    public class ScriptGenerationService : IScriptGenerationService
    {
        private readonly IConfigurationService _configurationService;
        private readonly ICredentialService _credentialService;
        private readonly string _templateDirectory;

        public ScriptGenerationService(IConfigurationService configurationService, ICredentialService credentialService, string templateDirectory = "script_templates")
        {
            _configurationService = configurationService;
            _credentialService = credentialService;
            _templateDirectory = templateDirectory;
        }

        public string GetPreviewCommand(RcloneJobConfig job)
        {
            if (job == null) throw new System.ArgumentNullException(nameof(job));

            var config = _configurationService.LoadConfig();
            if (config.Rclone == null) throw new System.ArgumentNullException(nameof(config.Rclone));

            var rcloneTemplatePath = Path.Combine(_templateDirectory, "rclone_pull_media.bat.template");

            if (File.Exists(rcloneTemplatePath))
            {
                var rcloneTemplate = File.ReadAllText(rcloneTemplatePath);
                var scriptContent = new StringBuilder(rcloneTemplate);
                scriptContent.Replace("%%RCLONE_EXE_PATH%%", Sanitize(config.Rclone.RclonePath ?? string.Empty));
                scriptContent.Replace("%%SOURCE%%", Sanitize(job.SourcePath ?? string.Empty));
                scriptContent.Replace("%%DEST%%", Sanitize(job.DestPath ?? string.Empty));
                scriptContent.Replace("%%LOG_DIR%%", Sanitize(config.Rclone.LogDir ?? string.Empty));
                scriptContent.Replace("%%FILTER_FILE%%", Sanitize(job.FilterFile ?? string.Empty));
                scriptContent.Replace("%%LOG_LEVEL%%", Sanitize(config.Rclone.LogLevel ?? "INFO"));
                scriptContent.Replace("%%MIN_AGE%%", Sanitize(config.Rclone.Flags?.MinAge ?? string.Empty));
                scriptContent.Replace("%%TRANSFERS%%", config.Rclone.Flags?.Transfers.ToString() ?? "4");
                scriptContent.Replace("%%CHECKERS%%", config.Rclone.Flags?.Checkers.ToString() ?? "8");

                return scriptContent.ToString();
            }

            throw new FileNotFoundException($"Template file not found at: {rcloneTemplatePath}");
        }

        public void GenerateScripts(AppConfig config, string? outputDirectory = null)
        {
            var outDir = outputDirectory ?? Path.Combine(System.AppContext.BaseDirectory, "scripts");

            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            // --- Generate Rclone Scripts ---
            var rcloneTemplatePath = Path.Combine(_templateDirectory, "rclone_pull_media.bat.template");
            if (File.Exists(rcloneTemplatePath))
            {
                var rcloneTemplate = File.ReadAllText(rcloneTemplatePath);
                var scriptContent = new StringBuilder(rcloneTemplate);
                if (config.Rclone != null)
                {
                    scriptContent.Replace("%%RCLONE_EXE_PATH%%", Sanitize(config.Rclone.RclonePath ?? string.Empty));
                    scriptContent.Replace("%%LOG_DIR%%", Sanitize(config.Rclone.LogDir ?? string.Empty));
                    scriptContent.Replace("%%LOG_LEVEL%%", Sanitize(config.Rclone.LogLevel ?? "INFO"));
                    scriptContent.Replace("%%MIN_AGE%%", Sanitize(config.Rclone.Flags?.MinAge ?? string.Empty));
                    scriptContent.Replace("%%TRANSFERS%%", config.Rclone.Flags?.Transfers.ToString() ?? "4");
                    scriptContent.Replace("%%CHECKERS%%", config.Rclone.Flags?.Checkers.ToString() ?? "8");
                }
                var outputPath = Path.Combine(outputDirectory, "rclone_pull_media.bat");
                File.WriteAllText(outputPath, scriptContent.ToString());
            }


            // --- Generate qB Cleanup Script ---
            if (config.QBittorrent != null)
            {
                var qbTemplatePath = Path.Combine(_templateDirectory, "qb_cleanup_ratio.ps1.template");
                if (File.Exists(qbTemplatePath))
                {
                    var qbTemplate = File.ReadAllText(qbTemplatePath);
                    var qbScriptContent = new StringBuilder(qbTemplate);

                    var qbUrl = $"{config.QBittorrent.Protocol}://{config.QBittorrent.Host}:{config.QBittorrent.Port}{config.QBittorrent.BasePath}";
                    qbScriptContent.Replace("%%QB_URL%%", qbUrl);
                    qbScriptContent.Replace("%%QB_USER%%", config.QBittorrent.Username ?? string.Empty);
                    var credential = _credentialService.RetrieveCredential("RcloneQBController_qBittorrent");
                    qbScriptContent.Replace("%%QB_PASS%%", credential?.Password ?? string.Empty);

                    if (config.Cleanup != null)
                    {
                        qbScriptContent.Replace("%%LOG_DIR%%", config.Cleanup.LogDir ?? string.Empty);
                        qbScriptContent.Replace("%%TARGET_RATIO%%", config.Cleanup.TargetRatio.ToString());
                        qbScriptContent.Replace("%%HNR_MINUTES%%", config.Cleanup.HnrMinutes.ToString());
                        qbScriptContent.Replace("%%MIN_AGE_MINS%%", config.Cleanup.MinAgeMinutes.ToString());

                        var categories = string.Join(", ", config.Cleanup.Categories?.Select(c => $"'{c}'") ?? Enumerable.Empty<string>());
                        qbScriptContent.Replace("%%CATEGORIES%%", categories);

                        var safeStates = string.Join(", ", config.Cleanup.SafeStates?.Select(s => $"'{s}'") ?? Enumerable.Empty<string>());
                        qbScriptContent.Replace("%%SAFE_STATES%%", safeStates);
                    }

                    var qbOutputPath = Path.Combine(outDir, "qb_cleanup_ratio.ps1");
                    File.WriteAllText(qbOutputPath, qbScriptContent.ToString());
                }
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