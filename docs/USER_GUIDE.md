# Rclone QB Controller User Guide

Welcome to Rclone QB Controller!
Hello! This application is designed to make your life easier by automating two key tasks:

Downloading completed files from your seedbox to your computer using a powerful tool called Rclone.

Cleaning up torrents from qBittorrent after they have finished seeding, so you don't have to do it manually.

This guide will walk you through the main features of the application.

## The Main Window
The application is split into two main sections:

*   **Control Panel (Left Side):** This is where you can see all your available tasks and control them.
*   **Activity Dashboard (Right Side):** This is a live feed that shows you exactly what the application is doing at any moment. When a download is in progress, you'll see a list of files being transferred right here.

## Rclone Jobs (Your Downloads)
This section lists all the download tasks you have set up, like "TV Shows" or "Movies". For each job, you have a few options:

*   **Run Now:** Click this button to start the download immediately. While a job is running, this button will change to a Stop button, which you can use to cancel the task.
*   **Preview:** If you want to see the technical command that will be run without actually starting it, you can click this button.
*   **Schedule Toggle (the switch on the right):** If you want a job to run automatically in the background, just flip this switch on. The application will then run it for you at the interval defined in the settings. Flip it off to disable the schedule for that specific job.

## qBittorrent Tasks (Cleaning Up)
This section is for managing the cleanup of old torrents in qBittorrent.

*   **Run Cleanup Script:** Click this to start the cleanup process based on the rules you've set (like the seed ratio).
*   **Dry Run:** This is a very useful safety feature. If you check the Dry Run box before clicking the button, the script will run in a simulation mode. It will show you a log of all the torrents it would have deleted, but it won't actually delete anything. This is a great way to test your rules and make sure they are working as you expect.

## System Tray Integration
The Rclone QB Controller can run silently in the background, accessible via a system tray icon.

*   **Minimize to Tray:** When you minimize the main application window, it will disappear from the taskbar and reside in the system tray. The application continues to run and perform scheduled tasks.
*   **Close to Tray:** By default, clicking the close button (X) on the main window will also minimize the application to the system tray instead of fully exiting. This ensures background operations continue uninterrupted.

To interact with the application when it's in the system tray, right-click the Rclone QB Controller icon to access the context menu:

*   **Show:** Click this option to restore the main application window from the system tray.
*   **Exit:** Select this option to completely close the application and stop all background processes.

## Notifications
The Rclone QB Controller provides toast notifications to keep you informed about the status of your background tasks.

*   **When Notifications Appear:** You will see a small pop-up notification in the corner of your screen when a script or job finishes, whether it completes successfully or encounters an error.
*   **Information Provided:** These notifications will typically include:
    *   The name of the completed task (e.g., "Rclone TV Job", "qBittorrent Cleanup").
    *   The outcome of the task (e.g., "Completed Successfully", "Failed").
    *   A brief summary or relevant details about the task's execution.

## Settings and Help
*   **Changing Settings:** You can change all the application's settings, including the schedule intervals, your qBittorrent connection details, and the cleanup rules, by going to the File -> Settings menu at the top of the window. Please note that sensitive information like passwords are securely stored in the Windows Credential Manager and are never saved directly in configuration files.
*   **Troubleshooting:** If something isn't working as expected, the log files are the best place to look for answers. You can easily open the folder containing all the log files by going to the Help -> Open Log Folder menu.