# Data Mapping Document

This document details the flow of information from the user interface (setup wizards and settings) to the `config.json` file and the final generated scripts.

| Wizard Input / UI Setting | `config.json` Path | Used In / Notes |
| :--- | :--- | :--- |
| **Rclone General** | | |
| Rclone executable path | `rclone.rclone_path` | Path to `rclone.exe`. Validated on startup. |
| Rclone remote name | `rclone.remote_name` | Name used in `rclone config create` and subsequent commands. |
| Rclone log directory | `rclone.log_dir` | Base directory for all rclone job logs. |
| Use JSON logs for rclone | `rclone.use_json_log` | Boolean flag. If true, `--use-json-log` is added to commands. |
| Rclone common flags | `rclone.flags` | Object containing flags like `min_age`, `transfers`, etc. |
| **Rclone Jobs** | | |
| Job definitions | `rclone.jobs` (array) | Each object defines a transfer job. |
| Job Name | `rclone.jobs[n].name` | User-friendly name (e.g., "tv"). |
| Job Source Path | `rclone.jobs[n].source_path` | Remote path for this job (e.g., `/path/to/tv`). |
| Job Destination Path | `rclone.jobs[n].dest_path` | Local destination path (e.g., `D:\Media\TV`). |
| Job Log Filename | `rclone.jobs[n].log` | Base filename for this job's log (e.g., `rclone_tv`). |
| **Seedbox Connection** | | |
| Seedbox Hostname | `seedbox.host` | Used for non-interactive `rclone config create`. |
| SFTP/SSH Port | `seedbox.port` | Used for non-interactive `rclone config create`. |
| Seedbox Username | `seedbox.username` | Used for non-interactive `rclone config create`. |
| Auth method | `seedbox.auth.method` | "password" or "ssh_key". |
| Password (obscured) | `seedbox.auth.pass_obscured` | Output of `rclone obscure`. |
| **VPN** | | |
| VPN config file path | `vpn.config_file` | Path to the `.ovpn` file. |
| Detected VPN client IP | `vpn.client_ip` | Automatically detected, but can be overridden. |
| **qBittorrent Connection** | | |
| Protocol | `qbittorrent.protocol` | "http" or "https". |
| Host IP | `qbittorrent.host` | Derived from VPN IP, but can be overridden. |
| WebUI Port | `qbittorrent.port` | Port for the qB Web UI. |
| Base Path | `qbittorrent.base_path` | Optional, for reverse proxy setups (e.g., `/qbittorrent`). |
| Username | `qbittorrent.username` | qB Web UI username. |
| Password Reference | `qbittorrent.password_ref` | Pointer to secure storage (e.g., `WindowsCredentialManager:qb_seedbox`). |
| **Cleanup Policy** | | |
| Target Categories | `cleanup.categories` | Array of categories to clean. Empty means all. |
| Target Ratio | `cleanup.target_ratio` | Minimum seed ratio. |
| Min Seed Time (Mins) | `cleanup.hnr_minutes` | Minimum seed time. |
| Min Age After Completion | `cleanup.min_age_minutes` | Grace period after download finishes. |
| Safe States | `cleanup.safe_states` | Torrent states exempt from deletion. |
| Deletion Mode | `cleanup.delete_mode` | "torrent_and_files" or "torrent_only". |
| Cleanup Log Directory | `cleanup.log_dir` | Directory for cleanup logs and CSVs. |
| Use Dated Logs | `cleanup.dated_logs` | Boolean flag to append date to log filenames. |
| **Scheduling** | | |
| Pull Frequency (Mins) | `schedule.pull_every_minutes` | Interval for running rclone jobs. |
| Cleanup Offset (Mins) | `schedule.cleanup_offset_minutes` | Delay after a pull job before running cleanup. |
| Run Only When Logged In | `schedule.only_when_logged_in` | Corresponds to a Windows Task Scheduler flag. |