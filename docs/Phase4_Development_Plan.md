# Detailed Technical Plan: Phase 4

This document outlines the technical implementation plan for the final phase of the RcloneQBController project. This phase focuses on implementing advanced features, including job scheduling, a comprehensive settings interface, system tray integration, and user notifications, to deliver a polished and feature-complete application.

## 1. Scheduling Service

### 1.1. Objective
To create a reliable, background scheduling service that automatically triggers rclone and qBittorrent jobs based on user-defined intervals.

### 1.2. Implementation Details
*   **File Creation:** A new service file, `SchedulingService.cs`, will be created in the `/Services` directory.
*   **Core Logic:** The service will utilize `System.Threading.Timer` for its scheduling mechanism. This provides a lightweight, thread-safe timer suitable for background tasks in a WPF application.
*   **Methods:**
    *   `public void Start(Job job)`: This method will initialize and start a new timer for a specific job (e.g., an rclone pull task). The timer's interval will be dynamically set based on the `pull_every_minutes` value for that job in `config.json`. The timer's callback delegate will be configured to invoke the `ScriptRunnerService`.
    *   `public void Stop(Job job)`: This method will find the corresponding active timer for the specified job and dispose of it, effectively stopping the schedule for that job.
*   **Integration:** The `MainViewModel` will be responsible for calling the `Start` and `Stop` methods of the `SchedulingService` when the user enables or disables a job's schedule from the UI. The service will be registered as a singleton in the application's dependency injection container to ensure a single source of truth for all scheduled tasks.

## 2. Settings Window

### 2.1. Objective
To build a comprehensive, user-friendly settings window that allows users to view and modify all aspects of the application's configuration.

### 2.2. Implementation Details
*   **File Creation:**
    *   A new view, `SettingsWindow.xaml`, will be created in the `/Views` directory.
    *   A corresponding view model, `SettingsViewModel.cs`, will be created in the `/ViewModels` directory.
*   **UI Structure:** The `SettingsWindow.xaml` will implement a `TabControl` as specified in the design document, with tabs for "qBittorrent," "Rclone," "Scheduling," and "Application." This organizes the settings into logical, manageable sections.
*   **ViewModel Logic (`SettingsViewModel.cs`):**
    *   **Loading:** Upon initialization, the ViewModel will use the `ConfigurationService` to load the current `AppConfig` object. The properties of this object will be bound to the various input controls (TextBoxes, CheckBoxes, etc.) in the `SettingsWindow.xaml`.
    *   **Saving:** A "Save" button in the window will be bound to an `ICommand` in the ViewModel. When invoked, this command will call the `ConfigurationService.SaveConfiguration()` method, passing the current state of the ViewModel's properties to persist the changes to `config.json`.
    *   **Validation:** The ViewModel will include data validation rules (e.g., for numeric inputs or required fields) to provide immediate feedback to the user.

## 3. User Guide & Menu Items

### 3.1. Objective
To provide users with easy access to help documentation and essential application functions through a standard menu bar.

### 3.2. Implementation Details
*   **User Guide Window:** A simple, read-only window, `UserGuideWindow.xaml`, will be created. It will contain a `TextBlock` or a similar control to display pre-defined help text, guiding the user on how to use the application.
*   **Main Window Menu:** A `<Menu>` control will be added to the `MainWindow.xaml`.
*   **Menu Item Actions:**
    *   **"Settings":** This menu item will be wired to open a new instance of the `SettingsWindow`.
    *   **"User Guide":** This will open a new instance of the `UserGuideWindow`.
    *   **"Open Log Folder":** This will use the `Process.Start()` method to open the log directory specified in `config.json` in the default file explorer, providing quick access for troubleshooting.

## 4. System Tray Integration

### 4.1. Objective
To allow the application to run unobtrusively in the background and be managed from the system tray.

### 4.2. Implementation Details
*   **Dependency:** The project will add a NuGet package reference to a well-established library for system tray icons, such as `Hardcodet.NotifyIcon.Wpf`.
*   **Icon Implementation:** An instance of the `TaskbarIcon` will be declared in `MainWindow.xaml`.
*   **Context Menu:** A context menu will be defined for the tray icon, providing essential actions:
    *   **"Show":** Brings the main window to the foreground.
    *   **"Exit":** Shuts down the application completely.
*   **Minimize-to-Tray Logic:** The `OnClosing` event of the `MainWindow` will be overridden. Instead of allowing the window to close, the event will be handled (`e.Cancel = true`), and the window's visibility will be set to `Hidden`. This action will minimize the application to the tray instead of terminating it. The "Exit" option in the context menu will be the primary method for closing the application.

## 5. Notifications

### 5.1. Objective
To provide clear, non-intrusive feedback to the user when background tasks (like a script run) are completed.

### 5.2. Implementation Details
*   **Dependency:** The project will use a library capable of showing modern toast notifications on Windows. The `Microsoft.Toolkit.Uwp.Notifications` library (for Windows 10/11) is the recommended choice for creating native-looking notifications.
*   **Integration:** The `ScriptRunnerService` will be updated. After a script process completes, the service will invoke a method that builds and displays a toast notification.
*   **Notification Content:** The notification will clearly state which script has finished and whether it succeeded or failed, providing at-a-glance status information to the user even when the application window is not visible.

## 6. Final Commit Point

Upon completion of all the features detailed in this plan, the work will be committed to the version control repository. The designated commit message for this body of work will be:

`feat: Implement scheduling, settings, system tray, and all remaining features`