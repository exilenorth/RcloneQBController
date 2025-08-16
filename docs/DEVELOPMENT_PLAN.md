# Phased Development Plan

This document outlines the phased development plan for the RcloneQBController application, breaking down the implementation into logical stages with clear goals, tasks, and commit points.

## Phase 1: Core Foundation & Services
*   **Goal:** Establish the project skeleton and the core, non-UI services required for the application to function.
*   **Key Tasks:** 
    *   Set up the .NET 8 WPF project.
    *   Implement the configuration service to read/write `config.json`.
    *   Create placeholder models and view models.
*   **Commit Point:** `feat: Initial project structure and core configuration services`

## Phase 2: First-Time User Experience
*   **Goal:** Implement the complete first-time setup wizard experience.
*   **Key Tasks:** 
    *   Build the UI for both the Rclone and qB Cleanup wizards.
    *   Implement the logic for dependency detection, user input validation, `rclone` remote creation, and dynamic script generation.
*   **Commit Point:** `feat: Implement first-time setup wizards for Rclone and qB Cleanup`

## Phase 3: Main Application & Script Execution
*   **Goal:** Build the main application window and implement the core script execution and monitoring functionality.
*   **Key Tasks:** 
    *   Develop the main window UI based on the design spec.
    *   Implement the script runner service.
    *   Build the graphical activity dashboard to parse and display real-time script output.
*   **Commit Point:** `feat: Implement main window UI and graphical script execution dashboard`

## Phase 4: Advanced Features & Polish
*   **Goal:** Implement all remaining features to complete the application.
*   **Key Tasks:** 
    *   Implement the scheduling service.
    *   Build the settings and user guide windows.
    *   Add system tray integration and notifications.
    *   Implement all menu bar actions.
*   **Commit Point:** `feat: Implement scheduling, settings, system tray, and all remaining features`