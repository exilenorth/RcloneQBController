# RcloneQBController

## Overview
A GUI application for managing and scheduling the `qb_cleanup_ratio.ps1` and `rclone_pull_media.bat` scripts.

## Core Features
*   Script Control & Scheduling
*   Real-Time Console View
*   In-App Settings Editor
*   User Guide
*   Log Folder Shortcut
*   VPN Status Indicator & Pre-Run Check
*   Dry Run UI Control
*   Configuration Validation
*   Dependency Management for Rclone

## Configuration (`config.json`)
```json
{
    "rclone": {
        "remote_name": "your_remote_name",
        "source_path": "path/to/your/source",
        "destination_path": "path/to/your/destination",
        "log_file": "path/to/your/rclone_log.log",
        "dry_run": false
    },
    "qbittorrent": {
        "cleanup_script_path": "path/to/your/qb_cleanup_ratio.ps1",
        "log_file": "path/to/your/qbittorrent_log.log"
    },
    "vpn": {
        "config_file": "path/to/your/vpn_config.ovpn",
        "credentials_file": "path/to/your/vpn_credentials.txt"
    },
    "scheduler": {
        "enabled": true,
        "interval_hours": 24
    }
}