# Settings Window Architectural Design

## 1. Introduction

This document outlines the architectural design for the "Settings" window of the RcloneQBController application. The design adheres to the existing MVVM pattern and addresses all core requirements, including a "Save/Cancel" user experience, live data validation, robust configuration lifecycle management, and a dedicated workflow for managing complex collections like Rclone jobs.

## 2. ViewModel Design: `SettingsViewModel`

The `SettingsViewModel` is the heart of the settings window. It will be responsible for managing a temporary copy of the application's configuration, handling user interactions, and performing real-time data validation.

### 2.1. Properties

*   **`ConfigCopy`**: An `AppConfig` object that is a deep copy of the main application configuration. This property will be the data source for all UI elements in the `SettingsWindow`.
*   **`RcloneJobs`**: An `ObservableCollection<RcloneJobConfig>` derived from `ConfigCopy.Rclone.Jobs` to facilitate dynamic updates in the `DataGrid`.
*   **`SelectedJob`**: An `RcloneJobConfig` property bound to the `SelectedItem` of the Rclone jobs `DataGrid`.

### 2.2. Commands

*   **`SaveCommand`**: An `ICommand` that is enabled only when the configuration is valid. When executed, it will:
    1.  Pass the `ConfigCopy` object to `ConfigurationService.Instance.SaveConfig()`.
    2.  Notify the `MainViewModel` to reload the configuration.
    3.  Close the `SettingsWindow`.
*   **`CancelCommand`**: An `ICommand` that discards all changes by simply closing the `SettingsWindow` without saving.
*   **`AddJobCommand`**: An `ICommand` that opens a dedicated "Add/Edit Job" dialog with a new `RcloneJobConfig` object.
*   **`EditJobCommand`**: An `ICommand` that is enabled only when a job is selected in the `DataGrid`. It opens the "Add/Edit Job" dialog with a copy of the `SelectedJob`.
*   **`RemoveJobCommand`**: An `ICommand` that is enabled only when a job is selected. It removes the `SelectedJob` from the `RcloneJobs` collection.

### 2.3. Data Validation (`IDataErrorInfo`)

The `SettingsViewModel` will implement the `IDataErrorInfo` interface to provide real-time validation feedback to the user.

*   **`Error`**: Returns a general error message (can be `null`).
*   **`this[string columnName]`**: Implements the indexer to return a validation error message for a specific property name.

The validation logic will be based on the rules defined in `docs/TECHNICAL_SPECIFICATION.md`:

*   **Path Validation**:
    *   `rclone.rclone_path` must exist.
    *   `rclone.log_dir` must be a valid path.
    *   `rclone.jobs[n].dest_path` must exist for each job.
    *   `vpn.config_file` must exist.
*   **Network Validation**:
    *   `qbittorrent.port` must be between 1 and 65535.
*   **Value Validation**:
    *   `cleanup.target_ratio` must be non-negative.
    *   `schedule.pull_every_minutes` must be a positive integer.
    *   `app_settings.log_retention_days` must be a positive integer.
    *   `rclone.jobs[n].max_runtime_minutes` must be a positive integer for each job.

The `SaveCommand`'s `CanExecute` logic will depend on the overall validation status of the `ConfigCopy`.

## 3. View Structure: `SettingsWindow.xaml`

The `SettingsWindow.xaml` will be structured to provide a clear and organized user experience.

```xaml
<Window ...>
    <Grid>
        <TabControl>
            <TabItem Header="General">
                <!-- Controls for AppSettings -->
            </TabItem>
            <TabItem Header="Rclone">
                <Grid>
                    <!-- TextBoxes for rclone_path, remote_name, log_dir -->
                    <DataGrid ItemsSource="{Binding RcloneJobs}" SelectedItem="{Binding SelectedJob}">
                        <!-- Columns for Name, Source, Destination, etc. -->
                    </DataGrid>
                    <StackPanel Orientation="Horizontal">
                        <Button Content="Add" Command="{Binding AddJobCommand}" />
                        <Button Content="Edit" Command="{Binding EditJobCommand}" />
                        <Button Content="Remove" Command="{Binding RemoveJobCommand}" />
                    </StackPanel>
                </Grid>
            </TabItem>
            <TabItem Header="qBittorrent">
                <!-- Controls for QBittorrentConfig and CleanupScriptConfig -->
            </TabItem>
            <TabItem Header="Scheduling">
                <!-- Controls for SchedulerConfig -->
            </TabItem>
        </TabControl>

        <StackPanel Orientation="Horizontal" HorizontalAlignment="Right" VerticalAlignment="Bottom">
            <Button Content="Save" Command="{Binding SaveCommand}" />
            <Button Content="Cancel" Command="{Binding CancelCommand}" />
        </StackPanel>
    </Grid>
</Window>
```

## 4. Dialog Management: "Add/Edit Job" Dialog

To manage the complexity of editing an `RcloneJobConfig` object, a separate dialog will be used.

*   **`JobEditorWindow.xaml` / `JobEditorViewModel.cs`**: A new View and ViewModel dedicated to editing a single `RcloneJobConfig` object.
*   **Workflow**:
    1.  The `SettingsViewModel`'s `AddJobCommand` or `EditJobCommand` creates an instance of `JobEditorViewModel`, passing a new or copied `RcloneJobConfig` object.
    2.  The `JobEditorWindow` is displayed as a modal dialog.
    3.  The `JobEditorViewModel` will also implement `IDataErrorInfo` for live validation of the job's properties.
    4.  The dialog will have "OK" and "Cancel" buttons.
    5.  If the user clicks "OK", the dialog returns `true`, and the `SettingsViewModel` updates its `RcloneJobs` collection with the new/modified job.
    6.  If the user clicks "Cancel", the dialog returns `false`, and no changes are made.

## 5. Data Flow Diagram: Configuration Lifecycle

The following diagram illustrates the lifecycle of the configuration data when the settings window is used.

```mermaid
sequenceDiagram
    participant MainViewModel
    participant SettingsWindow
    participant SettingsViewModel
    participant ConfigurationService

    MainViewModel->>SettingsWindow: Show Dialog
    activate SettingsWindow
    SettingsWindow->>ConfigurationService: LoadConfig()
    ConfigurationService-->>SettingsWindow: Returns AppConfig
    SettingsWindow->>SettingsViewModel: new SettingsViewModel(deepCopy(AppConfig))
    activate SettingsViewModel
    SettingsViewModel-->>SettingsWindow: Ready
    
    Note over SettingsWindow, SettingsViewModel: User edits the configuration copy
    
    alt User clicks Save
        SettingsWindow->>SettingsViewModel: Execute SaveCommand
        SettingsViewModel->>ConfigurationService: SaveConfig(ConfigCopy)
        ConfigurationService-->>SettingsViewModel: Save successful
        SettingsViewModel->>MainViewModel: Notify: ConfigSaved
        SettingsViewModel->>SettingsWindow: Close()
    else User clicks Cancel
        SettingsWindow->>SettingsViewModel: Execute CancelCommand
        SettingsViewModel->>SettingsWindow: Close()
    end
    
    deactivate SettingsViewModel
    deactivate SettingsWindow

    MainViewModel->>ConfigurationService: LoadConfig()
    ConfigurationService-->>MainViewModel: Returns updated AppConfig