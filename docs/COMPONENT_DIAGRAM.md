# Component Diagram

This diagram provides a high-level overview of the main components of the RcloneQBController application and their dependencies.

```mermaid
graph TD
    subgraph "User Interface"
        MainWindow_View
        ScriptControl_View
    end

    subgraph "Application Logic"
        MainViewModel
        SetupWizardViewModel
    end

    subgraph "Backend Services"
        ScriptRunnerService
        ScriptGenerationService
        ConfigurationService
    end

    subgraph "Data & Configuration"
        AppConfig_Model
        config_json[config.json]
    end

    subgraph "External Processes"
        rclone_scripts[rclone_pull_media.bat]
        qb_cleanup_script[qb_cleanup_ratio.ps1]
    end

    MainWindow_View --> MainViewModel
    MainViewModel --> ScriptRunnerService
    MainViewModel --> ScriptGenerationService
    MainViewModel --> ConfigurationService
    ScriptRunnerService --> rclone_scripts
    ScriptRunnerService --> qb_cleanup_script
    ScriptGenerationService --> config_json
    ConfigurationService --> config_json
    MainViewModel --> AppConfig_Model