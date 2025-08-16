# Detailed Technical Plan for Phase 3

This document outlines the development plan for Phase 3 of the RcloneQBController project, focusing on the implementation of the main application window and the graphical activity dashboard.

## 1. Main Window UI Development

The main window will be the primary user interface for the application. It will be implemented using a two-column layout as specified in the design document.

### 1.1. `MainWindow.xaml` Implementation

The `MainWindow.xaml` will be structured with a `Grid` containing two columns. The left column will host the script controls, and the right column will host the activity dashboard.

```xml
<Window x:Class="RcloneQBController.Views.MainWindow"
        ...>
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>

        <!-- Left Column: Script Controls -->
        <views:ScriptControlView Grid.Column="0" />

        <!-- Right Column: Activity Dashboard -->
        <views:ActivityDashboardView Grid.Column="1" />
    </Grid>
</Window>
```

### 1.2. `ScriptControlView.xaml` UserControl

A `UserControl` named `ScriptControlView.xaml` will be created for the left column. This control will be responsible for displaying the list of script jobs and providing controls for manual execution and scheduling. It will be bound to a corresponding `ScriptControlViewModel`.

### 1.3. `ActivityDashboardView.xaml` UserControl

A `UserControl` named `ActivityDashboardView.xaml` will be created for the right column. This control will contain the graphical log view and the dynamic file transfer view. It will be bound to the `ActivityDashboardViewModel`.

## 2. Script Runner Service

A `ScriptRunnerService.cs` will be created to handle the execution of the scripts.

### 2.1. `RunScriptAsync` Method

The primary method of this service will be `public async Task RunScriptAsync(Job job, Action<string> onOutput)`. This method will:

1.  **Instantiate `System.Diagnostics.Process`**: A new `Process` will be started for the appropriate script (`.bat` or `.ps1`).
2.  **Redirect Output**: Standard output and error streams will be redirected to be read by the application.
3.  **Asynchronous Reading**: The output will be read asynchronously line by line. For each line received, the `onOutput` callback will be invoked.
4.  **Concurrency Control**: A `Mutex` or an `isRunning` flag will be implemented to prevent concurrent executions of the same job. If a job is already running, any new requests to run it will be ignored.
5.  **Timeout Enforcement**: The service will start a timer based on the `max_runtime_minutes` for the job. If the process does not exit before the timer elapses, the service will terminate the process and log a timeout error.

## 3. Activity Dashboard Implementation

The activity dashboard will provide real-time feedback on script execution.

### 3.1. `ActivityDashboardViewModel.cs`

The `ActivityDashboardViewModel.cs` will contain the logic for the dashboard. It will have two main properties:

*   `public ObservableCollection<LogEntry> LogEntries { get; }`
*   `public ObservableCollection<FileTransfer> FileTransfers { get; }`

### 3.2. Output Parsing

The ViewModel will receive the raw string output from the `ScriptRunnerService` via the `onOutput` callback. It will then parse this output:

*   **For `rclone` jobs**: If `rclone.use_json_log` is true, the ViewModel will deserialize the JSON log lines into objects. These objects will be used to create or update `LogEntry` and `FileTransfer` objects in the `ObservableCollection`s.
*   **For `qb_cleanup` jobs**: The plain text lines will be parsed to create `LogEntry` objects.

The use of `ObservableCollection` will ensure that the UI is automatically updated as new log entries and file transfers are added or updated.

## 4. Final Commit Point

The goal of this phase is to commit all the above work under a single, comprehensive feature commit.

**Commit Message:** `feat: Implement main window UI and graphical script execution dashboard`