# RcloneQBController

## Overview
A GUI application for managing and scheduling the `qb_cleanup_ratio.ps1` and `rclone_pull_media.bat` scripts.

## Core Features
*   Script Control & Scheduling
*   Graphical Activity Dashboard
*   Centralized Settings Management via UI
*   **Settings Window:** A dedicated user interface for comprehensive application configuration, eliminating the need for manual `config.json` edits.
*   User Guide
*   Log Folder Shortcut
*   VPN Status Indicator & Pre-Run Check
*   Dry Run UI Control
*   Configuration Validation
*   Dependency Management for Rclone
*   First-Time Setup Wizards

## Technology Stack
The RcloneQBController is built with:
*   **Language:** C#
*   **Framework:** .NET 9
*   **UI:** Windows Presentation Foundation (WPF)

## Configuration
All application settings are now managed through the dedicated **Settings Window** within the UI, providing a user-friendly interface for configuration. While a `config.json` file still exists, it is primarily managed by the application and manual edits are generally not required. The structure of the underlying configuration is as follows:

```json
{
  "app_settings": {
    "log_retention_days": 14
  },
  "rclone": {
    "rclone_path": "C:\\Rclone\\rclone.exe",
    "remote_name": "seedbox",
    "log_dir": "C:\\Rclone\\logs",
    "use_json_log": true,
    "flags": { "min_age": "5m", "transfers": 6, "checkers": 8, "update": true },
    "jobs": [
      { "name": "tv",     "source_path": "/home/USER/torrents/qbittorrent/Media/TV",     "dest_path": "D:\\Media\\TV",     "log": "rclone_tv" },
      { "name": "movies", "source_path": "/home/USER/torrents/qbittorrent/Media/Movies", "dest_path": "D:\\Media\\Movies", "log": "rclone_movies" }
    ]
  },
  "seedbox": {
    "host": "58.nl21.seedit4.me",
    "port": 2102,
    "username": "seedit4me",
    "auth": { "method": "password", "pass_obscured": "XXXX" }
  },
  "vpn": {
    "config_file": "C:\\Users\\Matt\\Downloads\\seedbox.ovpn",
    "client_ip": "10.8.0.2"
  },
  "qbittorrent": {
    "protocol": "http",
    "host": "10.8.0.1",
    "port": 9148,
    "base_path": "",
    "username": "seedit4me",
    "password_ref": "WindowsCredentialManager:qb_seedbox"
  },
  "cleanup": {
    "categories": [],
    "target_ratio": 2.0,
    "hnr_minutes": 14400,
    "min_age_minutes": 30,
    "safe_states": ["paused","stalledUP","error","missingFiles"],
    "delete_mode": "torrent_and_files",
    "log_dir": "C:\\Rclone\\logs",
    "dated_logs": true
  },
  "schedule": {
    "pull_every_minutes": 15,
    "cleanup_offset_minutes": 5,
    "only_when_logged_in": true
  }
}
```

## Development Roadmap
The development of RcloneQBController is structured into four key phases:

*   **Phase 1: Core Foundation & Services:** Establish the project skeleton and the core, non-UI services required for the application to function.
*   **Phase 2: First-Time User Experience:** Implement the complete first-time setup wizard experience, including dependency detection, user input validation, rclone remote creation, and dynamic script generation.
*   **Phase 3: Main Application & Script Execution:** Build the main application window and implement the core script execution and monitoring functionality, including the graphical activity dashboard.
*   **Phase 4: Advanced Features & Polish:** Implement all remaining features to complete the application, such as the scheduling service, settings and user guide windows, system tray integration, and notifications.