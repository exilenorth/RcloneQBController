# Detailed Technical Plan: Phase 4

This document outlines the technical implementation plan for the final phase of the RcloneQBController project. This phase focuses on implementing advanced features, including job scheduling, a comprehensive settings interface, system tray integration, and user notifications, to deliver a polished and feature-complete application.

## 1. Scheduling Service (Completed)

### 1.1. Objective
To create a reliable, background scheduling service that automatically triggers rclone and qBittorrent jobs based on user-defined intervals.

### 1.2. Implementation Details
*   **File Creation:** A new service file, `SchedulingService.cs`, will be created in the `/Services` directory.
*   **Core Logic:** The service will utilize `System.Threading.Timer` for its scheduling mechanism. This provides a lightweight, thread-safe timer suitable for background tasks in a WPF application.
*   **Methods:**
    *   `public void Start(Job job)`: This method will initialize and start a new timer for a specific job (e.g., an rclone pull task). The timer's interval will be dynamically set based on the `schedule.pull_every_minutes` value in `config.json`. The timer's callback delegate will be configured to invoke the `ScriptRunnerService`.
    *   `public void Stop(Job job)`: This method will find the corresponding active timer for the specified job and dispose of it, effectively stopping the schedule for that job.
*   **Integration:** The `MainViewModel` will be responsible for calling the `Start` and `Stop` methods of the `SchedulingService` when the user enables or disables a job's schedule from the UI. The service will be registered as a singleton in the application's dependency injection container to ensure a single source of truth for all scheduled tasks.
*   **Catch-Up Logic:** The `SchedulingService` will be updated to detect if the application was asleep when a run was scheduled. If a scheduled run is missed, the service will trigger an immediate "catch-up" run to ensure data synchronization is not delayed.

## 2. Settings Window (Completed)

### 2.1. Objective
To build a comprehensive, user-friendly settings window that allows users to view and modify all aspects of the application's configuration.

### 2.2. Implementation Summary
The Settings Window has been successfully implemented, providing a centralized and intuitive user interface for managing all application configurations. This includes dedicated tabs for qBittorrent, Rclone, Scheduling, and Application settings, ensuring a logical organization of configurable parameters. The `SettingsWindow.xaml` and `SettingsViewModel.cs` were created as planned, with the ViewModel handling the loading, saving, and validation of configuration data via the `ConfigurationService`. This feature significantly enhances user experience by eliminating the need for manual `config.json` edits and providing immediate feedback through integrated validation.

## 3. User Guide & Menu Items (Completed)

### 3.1. Objective
To provide users with easy access to help documentation and essential application functions through a standard menu bar.

### 3.2. Implementation Details
The User Guide and Menu Items feature has been successfully implemented. A new `UserGuideWindow.xaml` was created to display help documentation. The `MainWindow.xaml` now includes a menu bar with options for "Settings", "User Guide", and "Open Log Folder". These menu items are wired to open the respective windows or execute the `Process.Start()` method for opening the log directory, providing intuitive navigation and quick access to essential application functions.

## 4. System Tray Integration (Completed)

### 4.1. Objective
To allow the application to run unobtrusively in the background and be managed from the system tray.

### 4.2. Implementation Details
*   **Dependency:** The project will add a NuGet package reference to a well-established library for system tray icons, such as `Hardcodet.NotifyIcon.Wpf`.
*   **Icon Implementation:** An instance of the `TaskbarIcon` will be declared in `MainWindow.xaml`.
*   **Context Menu:** A context menu will be defined for the tray icon, providing essential actions:
    *   **"Show":** Brings the main window to the foreground.
    *   **"Exit":** Shuts down the application completely.
*   **Minimize-to-Tray Logic:** The `OnClosing` event of the `MainWindow` will be overridden. Instead of allowing the window to close, the event will be handled (`e.Cancel = true`), and the window's visibility will be set to `Hidden`. This action will minimize the application to the tray instead of terminating it. The "Exit" option in the context menu will be the primary method for closing the application.

## 5. Notifications (Completed)

### 5.1. Objective
To provide clear, non-intrusive feedback to the user when background tasks (like a script run) are completed.

### 5.2. Implementation Details
*   **Dependency:** The project will use a library capable of showing modern toast notifications on Windows. The `Microsoft.Toolkit.Uwp.Notifications` library (for Windows 10/11) is the recommended choice for creating native-looking notifications.
*   **Integration:** The `ScriptRunnerService` will be updated. After a script process completes, the service will invoke a method that builds and displays a toast notification.
*   **Notification Content:** The notification will clearly state which script has finished and whether it succeeded or failed, providing at-a-glance status information to the user even when the application window is not visible.

## 6. User-Friendly Error Handling (Completed)

### 6.1. Objective
To abstract and simplify error reporting, presenting users with clear, actionable messages instead of technical exception details.

### 6.2. Implementation Details
*   **File Creation:** A new service, `UserNotifierService.cs`, will be created in the `/Services` directory.
*   **Core Logic:** This service will contain a method (e.g., `ShowFriendlyError(Exception ex)`) that maps specific exception types (e.g., `IOException`, `InvalidOperationException`) to pre-defined, user-friendly strings sourced from `TECHNICAL_SPECIFICATION.md`.
*   **Integration:** Key operations within services like `ScriptRunnerService` will be wrapped in `try-catch` blocks. The `catch` block will pass the exception to the `UserNotifierService` to handle the user-facing notification, ensuring consistent and helpful error feedback throughout the application.

## 7. Final Commit Point

Upon completion of all the features detailed in this plan, the work will be committed to the version control repository. The designated commit message for this body of work will be:

`feat: Implement scheduling, settings, tray, and enhanced error handling`