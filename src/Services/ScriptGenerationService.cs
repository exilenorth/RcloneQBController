using RcloneQBController.Models;
using System.IO;
using System.Text;

namespace RcloneQBController.Services
{
    public class ScriptGenerationService
    {
        public void GenerateScripts(AppConfig config)
        {
            // Generate rclone_pull_media.bat
            var rcloneTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "script_templates", "rclone_pull_media.bat.template");
            var rcloneScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "scripts", "rclone_pull_media.bat");
            var rcloneTemplate = File.ReadAllText(rcloneTemplatePath);

            var rcloneScriptContent = new StringBuilder(rcloneTemplate);
            // Replace placeholders
            // ...

            File.WriteAllText(rcloneScriptPath, rcloneScriptContent.ToString());

            // Generate qb_cleanup_ratio.ps1
            var qbTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "script_templates", "qb_cleanup_ratio.ps1.template");
            var qbScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "scripts", "qb_cleanup_ratio.ps1");
            var qbTemplate = File.ReadAllText(qbTemplatePath);

            var qbScriptContent = new StringBuilder(qbTemplate);
            // Replace placeholders
            // ...

            File.WriteAllText(qbScriptPath, qbScriptContent.ToString());
        }
    }
}