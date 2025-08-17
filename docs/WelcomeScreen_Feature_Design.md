# Welcome Screen Feature Design

## 1. Overview

This document outlines the design for a new "Welcome" screen to be added to the `SetupWizardWindow`. The purpose of this screen is to provide a friendly introduction to the application for new, non-technical users. It will only be displayed when the application is run for the first time and the `config.json` file is not present.

## 2. Triggering Condition

The Welcome screen and the subsequent setup wizard will be displayed if the following condition is met at application startup:

- `!File.Exists("config.json")`

This logic is located in `src/App.xaml.cs`.

## 3. Layout

The Welcome screen will have a simple, clean layout with a vertical arrangement of elements.

- **Container**: A `Grid` with a `StackPanel` inside to center the content.
- **Styling**: The design will be consistent with the existing wizard's theme (dark background, light text).

```xml
<UserControl x:Class="RcloneQBController.Views.WelcomeView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Grid>
        <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center" Margin="20">
            <TextBlock Text="{Binding Title}" FontSize="24" FontWeight="Bold" Foreground="#F1F1F1" Margin="0,0,0,20" TextWrapping="Wrap" TextAlignment="Center"/>
            <TextBlock Text="{Binding Description}" FontSize="16" Foreground="#F1F1F1" Margin="0,0,0,20" TextWrapping="Wrap" TextAlignment="Center"/>
            <TextBlock Text="{Binding SetupOverview}" FontSize="14" Foreground="#CCCCCC" TextWrapping="Wrap" TextAlignment="Center"/>
        </StackPanel>
    </Grid>
</UserControl>
```

## 4. Content

The following text will be displayed on the Welcome screen:

- **Title**: "Welcome to Rclone QB Controller"
- **Description**: "This application helps you automate file transfers from your seedbox. It will download your completed files and clean up the torrents when they are finished seeding."
- **Setup Overview**: "This wizard will guide you through the setup process. Click 'Next' to get started."

## 5. ViewModel

A new `WelcomeViewModel.cs` file will be created in `src/ViewModels/`.

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RcloneQBController.ViewModels
{
    public class WelcomeViewModel : INotifyPropertyChanged
    {
        public string Title { get; } = "Welcome to Rclone QB Controller";
        public string Description { get; } = "This application helps you automate file transfers from your seedbox. It will download your completed files and clean up the torrents when they are finished seeding.";
        public string SetupOverview { get; } = "This wizard will guide you through the setup process. Click 'Next' to get started.";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

## 6. Integration

The new Welcome screen will be integrated into the `SetupWizardViewModel` as the first step.

- **File to modify**: `src/ViewModels/SetupWizardViewModel.cs`
- **Change**: Add `new WelcomeViewModel()` to the beginning of the `_steps` list.

```csharp
// In SetupWizardViewModel.cs

public SetupWizardViewModel()
{
    _steps = new List<object>
    {
        new WelcomeViewModel(), // Add this line
        new RcloneInstallViewModel(),
        new SeedboxConnectionViewModel(),
        new TransferJobViewModel(),
        new RcloneSummaryViewModel(),
        new OpenVPNViewModel(),
        new QbittorrentViewModel(),
        new CleanupSummaryViewModel()
    };

    _currentStepIndex = 0;
    CurrentStepViewModel = _steps[_currentStepIndex];

    // ... rest of the constructor
}
```

## 7. Workflow Diagram

```mermaid
graph TD
    A[Start] --> B{config.json exists?};
    B -- No --> C[Show Welcome Screen];
    B -- Yes --> D[Show Main Window];
    C --> E{Next};
    E --> F[Rclone Install];
    F --> G[...];