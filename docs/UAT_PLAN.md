# User Acceptance Testing (UAT) Plan

## 1. Introduction
This User Acceptance Testing (UAT) plan outlines a series of real-world scenarios designed to validate the RcloneQBController application from an end-user perspective. The primary goal of this UAT phase is to ensure that the application meets the specified business requirements and user expectations, and is ready for release. This document provides a step-by-step checklist for human testers to follow, covering core functionalities and critical edge cases.

## 2. Scenario A: The New User ("Happy Path")

This scenario guides a new user through the initial setup and typical daily operations, ensuring a smooth and intuitive experience.

### Setup and Initial Configuration
- [ ] **Start Clean**:
    - [ ] Ensure no previous configuration files exist (e.g., `config.json`, `credentials.json`).
    - [ ] Launch the application.
    - [ ] Verify the Welcome Screen is displayed.
- [ ] **Complete Setup Wizard**:
    - [ ] Follow the on-screen prompts to configure Rclone, qBittorrent, and VPN settings.
    - [ ] Provide valid credentials and paths.
    - [ ] Verify that all settings are saved correctly after completing the wizard.
    - [ ] Confirm that the application transitions to the main dashboard after setup.

### Core Functionality Testing
- [ ] **Manual Job Execution (Rclone Pull)**:
    - [ ] Navigate to the Rclone Transfer section.
    - [ ] Configure a new Rclone pull job with valid source and destination paths.
    - [ ] Initiate the job manually.
    - [ ] Monitor the job progress in the Activity Dashboard.
    - [ ] Verify that files are transferred successfully to the specified destination.
    - [ ] Check the Rclone log for successful completion messages.
- [ ] **Manual Job Execution (qBittorrent Cleanup)**:
    - [ ] Navigate to the qBittorrent Cleanup section.
    - [ ] Configure a new cleanup job (e.g., by ratio or by tag).
    - [ ] Initiate the job manually.
    - [ ] Monitor the job progress in the Activity Dashboard.
    - [ ] Verify that torrents are removed from qBittorrent as expected.
    - [ ] Check the qBittorrent log for successful cleanup messages.
- [ ] **Scheduled Job Execution**:
    - [ ] Configure an Rclone pull job to run on a schedule (e.g., daily at a specific time).
    - [ ] Configure a qBittorrent cleanup job to run on a different schedule.
    - [ ] Verify that the scheduler service is running.
    - [ ] Observe the jobs executing automatically at their scheduled times.
    - [ ] Confirm successful completion of scheduled jobs in the Activity Dashboard and respective logs.

### Post-Operation Verification
- [ ] **Cleanup Script Usage**:
    - [ ] Run the integrated cleanup script (if applicable) to remove temporary files or logs.
    - [ ] Verify that the cleanup script executes without errors.
    - [ ] Confirm that specified files/directories are cleaned up.
- [ ] **Verify Settings Persistence**:
    - [ ] Close the application.
    - [ ] Relaunch the application.
    - [ ] Verify that all previously configured settings (Rclone, qBittorrent, VPN, schedules) are loaded correctly and persist across sessions.

## 3. Scenario B: Edge Cases and Error Conditions

This scenario focuses on testing the application's robustness and error handling capabilities under various challenging conditions.

### Invalid Configurations and Paths
- [ ] **Invalid Rclone Path**:
    - [ ] In settings, configure an invalid Rclone executable path.
    - [ ] Attempt to run an Rclone job.
    - [ ] Verify that an appropriate error message is displayed to the user.
    - [ ] Confirm that the job fails gracefully without crashing the application.
- [ ] **Invalid qBittorrent Credentials/URL**:
    - [ ] In settings, provide incorrect qBittorrent API URL or credentials.
    - [ ] Attempt to run a qBittorrent cleanup job.
    - [ ] Verify that an authentication/connection error is displayed.
    - [ ] Confirm that the application handles the error without unexpected behavior.
- [ ] **Non-existent Source/Destination Paths**:
    - [ ] Configure an Rclone job with a source or destination path that does not exist.
    - [ ] Initiate the job.
    - [ ] Verify that Rclone reports an error related to the invalid path.
    - [ ] Confirm that the application displays this error clearly.

### Concurrent Operations and System Behavior
- [ ] **Concurrent Job Execution**:
    - [ ] Start an Rclone transfer job.
    - [ ] While the Rclone job is running, attempt to start a qBittorrent cleanup job.
    - [ ] Verify that both jobs can run concurrently without interference or performance degradation.
    - [ ] Monitor the Activity Dashboard to ensure both jobs' statuses are updated correctly.
- [ ] **Application Minimization/System Tray**:
    - [ ] Start a long-running Rclone job.
    - [ ] Minimize the application to the system tray.
    - [ ] Verify that the job continues to run in the background.
    - [ ] Restore the application from the system tray.
    - [ ] Confirm that the Activity Dashboard accurately reflects the ongoing job's status.
- [ ] **Abrupt Application Closure**:
    - [ ] Start a job.
    - [ ] Force-close the application (e.g., via Task Manager) while a job is running.
    - [ ] Relaunch the application.
    - [ ] Verify that the application starts without corruption.
    - [ ] Check if any recovery mechanisms or error logs are present.

### Network and Connectivity Issues
- [ ] **Network Disconnection during Transfer**:
    - [ ] Start an Rclone transfer job.
    - [ ] Disconnect the network connection during the transfer.
    - [ ] Verify that Rclone reports a network error.
    - [ ] Confirm that the application handles the disconnection gracefully (e.g., pauses, retries, or reports failure).
    - [ ] Reconnect the network and observe if the job resumes or needs to be restarted.
- [ ] **VPN Connection Failure**:
    - [ ] Configure VPN settings.
    - [ ] Attempt to connect to a VPN with incorrect credentials or a non-existent server.
    - [ ] Verify that an appropriate error message is displayed regarding the VPN connection failure.
    - [ ] Confirm that the application does not proceed with jobs that require a VPN connection if the VPN fails to establish.