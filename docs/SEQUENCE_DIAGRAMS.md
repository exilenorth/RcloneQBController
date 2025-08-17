# Sequence Diagrams

This document contains sequence diagrams for key user flows in the RcloneQBController application.

## User Clicks "Run Now"

This diagram shows the sequence of events when a user initiates a job manually.

```mermaid
sequenceDiagram
    participant User
    participant ScriptControlView
    participant MainViewModel
    participant ScriptRunnerService
    participant RcloneScript

    User->>ScriptControlView: Clicks "Run Now"
    ScriptControlView->>MainViewModel: Executes RunJobCommand
    MainViewModel->>ScriptRunnerService: Calls RunRcloneJobAsync()
    ScriptRunnerService->>RcloneScript: Starts Process
    RcloneScript-->>ScriptRunnerService: Returns output line
    ScriptRunnerService-->>MainViewModel: Invokes callback with output
    MainViewModel-->>ScriptControlView: Updates ActivityDashboard
```

## Application First-Time Start

This diagram illustrates the flow when the application is launched for the first time and the configuration wizard is shown.

```mermaid
sequenceDiagram
    participant App.xaml.cs
    participant ConfigurationService
    participant SetupWizardWindow
    participant SetupWizardViewModel
    participant ScriptGenerationService

    App.xaml.cs->>ConfigurationService: CheckIfConfigExists()
    ConfigurationService-->>App.xaml.cs: Returns false
    App.xaml.cs->>SetupWizardWindow: ShowDialog()
    SetupWizardWindow->>SetupWizardViewModel: Manages wizard steps
    SetupWizardViewModel->>ConfigurationService: SaveConfiguration()
    SetupWizardViewModel->>ScriptGenerationService: GenerateScripts()
    ScriptGenerationService-->>SetupWizardViewModel: Scripts created
    ConfigurationService-->>SetupWizardViewModel: Configuration saved
    SetupWizardViewModel-->>SetupWizardWindow: Closes wizard
```

## Setup Wizard Saves Configuration

This diagram details the final step of the setup wizard where the configuration is saved and scripts are generated.

```mermaid
sequenceDiagram
    participant User
    participant SetupWizardWindow
    participant SetupWizardViewModel
    participant ConfigurationService
    participant ScriptGenerationService

    User->>SetupWizardWindow: Clicks "Finish"
    SetupWizardWindow->>SetupWizardViewModel: Executes SaveConfigurationCommand
    SetupWizardViewModel->>ConfigurationService: Saves user settings to config.json
    ConfigurationService-->>SetupWizardViewModel: Returns success
    SetupWizardViewModel->>ScriptGenerationService: Generates .bat and .ps1 scripts from templates
    ScriptGenerationService-->>SetupWizardViewModel: Returns success
    SetupWizardViewModel-->>SetupWizardWindow: Closes the wizard