# Detailed Technical Plan: Phase 1

This document outlines the specific, actionable technical steps required to complete Phase 1 of the RcloneQBController project. The goal of this phase is to establish the core project structure, implement configuration management, and create the basic application shell.

## 1. Project & Solution Setup

**Objective:** Create the foundational Visual Studio solution and project files, and establish the MVVM directory structure.

**Steps:**

1.  **Create Solution:**
    *   Generate a new blank solution file named `RcloneQBController.sln` in the root directory.

2.  **Create WPF Project:**
    *   Create a new WPF Project file named `src/RcloneQBController.csproj`.
    *   Set the `TargetFramework` property in the `.csproj` file to `net9.0-windows`.

3.  **Establish Directory Structure:**
    *   Create the `script_templates/` directory in the project root to store script blueprints.
    *   Within the `src/` directory, create the following subdirectories to adhere to the MVVM pattern:
        *   `Assets`
        *   `Models`
        *   `Services`
        *   `ViewModels`
        *   `Views`

## 2. Configuration Management Implementation

**Objective:** Implement the necessary models and services to load, manage, and save application settings from `config.json`.

### 2.1. Configuration Models

**Objective:** Define the C# class structure that maps directly to the `config.json` file.

**Steps:**

1.  **Create Model Files:** Inside the `src/Models/` directory, create the following C# class files. The properties within these classes must match the keys and structure defined in the `TECHNICAL_SPECIFICATION.md`.

    *   `AppConfig.cs`: The root configuration object.
    *   `RcloneConfig.cs`
    *   `RcloneJobConfig.cs`
    *   `SeedboxConfig.cs`
    *   `VpnConfig.cs`
    *   `QBittorrentConfig.cs`
    *   `CleanupScriptConfig.cs`
    *   `SchedulerConfig.cs`

### 2.2. Configuration Service

**Objective:** Create a service to handle the logic of reading from and writing to the `config.json` file.

**Steps:**

1.  **Create Service File:**
    *   Create a new file named `ConfigurationService.cs` inside the `src/Services/` directory.

2.  **Implement Service Logic:**
    *   The class should be implemented as a singleton to ensure a single source of configuration data.
    *   It must utilize the `System.Text.Json` library for serialization and deserialization.
    *   Define the following public methods:
        *   `public AppConfig LoadConfig()`: This method will read `config.json` from the disk, deserialize it into the `AppConfig` object model, and return the object.
        *   `public void SaveConfig(AppConfig config)`: This method will take an `AppConfig` object, serialize it into a JSON string with appropriate formatting, and write it back to the `config.json` file.

## 3. Core Application Shell

**Objective:** Create the main application entry point and the primary window, establishing the basic UI foundation.

**Steps:**

1.  **Create App.xaml:**
    *   Create the `App.xaml` and its code-behind file `App.xaml.cs`. This will serve as the application's entry point.

2.  **Create MainWindow:**
    *   In the `src/Views/` directory, create the main application window file `MainWindow.xaml` and its code-behind `MainWindow.xaml.cs`.

3.  **Create MainViewModel:**
    *   In the `src/ViewModels/` directory, create a placeholder class `MainViewModel.cs`. This class will later contain the logic and data bindings for the `MainWindow`.

## 4. Final Commit Point

**Objective:** Conclude this phase with a clean, well-defined commit that encapsulates all the foundational work.

**Action:**

*   After completing all the steps above, commit all new and modified files to the repository with the following commit message:
    *   `feat: Initial project structure and core configuration services`