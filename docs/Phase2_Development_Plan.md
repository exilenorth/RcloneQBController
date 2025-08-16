# Detailed Technical Plan: Phase 2 - First-Time Setup Wizards

## 1. Wizard View & ViewModel Foundation

This section outlines the creation of the core components for the wizard interface, following the MVVM pattern.

### 1.1. Main Wizard Window
- **File:** `src/RcloneQBController/Views/SetupWizardWindow.xaml`
- **Purpose:** To act as the primary container for all wizard steps.
- **Implementation:**
    - A `Window` with a `ContentControl` that will dynamically display the current wizard step.
    - The `DataContext` will be an instance of `SetupWizardViewModel`.
    - Navigation buttons (Back, Next, Finish) will be placed in a consistent footer area.

### 1.2. Main Wizard ViewModel
- **File:** `src/RcloneQBController/ViewModels/SetupWizardViewModel.cs`
- **Purpose:** To manage the overall state and flow of the setup wizards.
- **Implementation:**
    - `CurrentStepViewModel`: A property holding the ViewModel of the currently active `UserControl`.
    - `NextCommand`: A `RelayCommand` that advances to the next step in the sequence.
    - `BackCommand`: A `RelayCommand` that returns to the previous step.
    - Logic to control the visibility and enabled state of navigation buttons based on the current step.

### 1.3. Wizard Step Views & ViewModels
- **Purpose:** To create a modular and maintainable structure for the wizard steps.
- **Implementation:**
    - For each step in both wizards, a dedicated `UserControl` (View) and a corresponding ViewModel will be created.
    - **Rclone Wizard:**
        - `RcloneInstallView.xaml` & `RcloneInstallViewModel.cs`
        - `SeedboxConnectionView.xaml` & `SeedboxConnectionViewModel.cs`
        - `TransferJobView.xaml` & `TransferJobViewModel.cs`
        - `RcloneSummaryView.xaml` & `RcloneSummaryViewModel.cs`
    - **qB Cleanup Wizard:**
        - `OpenVPNView.xaml` & `OpenVPNViewModel.cs`
        - `QbittorrentView.xaml` & `QbittorrentViewModel.cs`
        - `CleanupSummaryView.xaml` & `CleanupSummaryViewModel.cs`

## 2. Rclone Setup Implementation

### 2.1. Rclone Installation Step
- **ViewModel:** `RcloneInstallViewModel.cs`
- **Logic:**
    - On initialization, the ViewModel will follow the detailed detection workflow specified in the `TECHNICAL_SPECIFICATION.md`:
      - Check `rclone_path` in `config.json`.
      - Validate the path if it exists.
      - Search the system `PATH` if the configured path is invalid or not found.
    - A property will bind to the user's selection (Yes/No).
    - A hyperlink to the rclone downloads page will be available.
    - The "Next" command will be enabled once the user confirms installation.

### 2.2. Seedbox Connection Step
- **ViewModel:** `SeedboxConnectionViewModel.cs`
- **Logic:**
    - Properties will be created for `Host`, `Port`, `Username`, `Password`, and `RemoteName`.
    - The ViewModel will implement the secure password flow defined in the `TECHNICAL_SPECIFICATION.md`: it will call `rclone obscure` and use the captured output for the final `rclone config create` command, ensuring no plaintext passwords are ever stored.
    - The "Test Connection" command will construct and run `rclone lsd [remote]:` non-interactively, capturing the output to determine success or failure.
    - Upon successful completion of the wizard, the ViewModel will orchestrate the final `rclone config create` command.

## 3. qB Cleanup Setup Implementation

### 3.1. OpenVPN Step
- **ViewModel:** `OpenVPNViewModel.cs`
- **Logic:**
    - The ViewModel will check for the existence of `openvpn.exe` at its default installation path.
    - It will handle the file selection logic for the `.ovpn` configuration file, copying it to the appropriate user directory.
    - A "Test VPN" command will call the reusable `VpnService.TestApi()` method to confirm connectivity.

### 3.2. qBittorrent Step
- **ViewModel:** `QbittorrentViewModel.cs`
- **Logic:**
    - A "Find my VPN IP" command will execute `ipconfig`, parse the output to find the VPN adapter's IP, and derive the gateway IP (e.g., `10.8.0.2` -> `10.8.0.1`).
    - Properties for `Host`, `Port`, `Username`, and `Password` will be available.
    - The "Test Connection" command will call the reusable `VpnService.TestApi()` method to confirm connectivity.
    - Credentials will be securely stored in the Windows Credential Manager.

## 4. Script Generation Service

### 4.1. Service Definition
- **File:** `src/RcloneQBController/Services/ScriptGenerationService.cs`
- **Purpose:** To centralize the logic for creating the final executable scripts from templates.

### 4.2. Primary Method
- **Signature:** `public void GenerateScripts(AppConfig config)`
- **Logic:**
    1.  The service will be invoked after the wizards are successfully completed.
    2.  It will read the `rclone_pull_media.bat.template` and `qb_cleanup_ratio.ps1.template` files.
    3.  Using the final `AppConfig` object, it will perform a series of string replacements on the template content, substituting placeholders (e.g., `{{RCLONE_REMOTE}}`, `{{QBITTORRENT_HOST}}`) with the actual configuration values.
    4.  The resulting strings will be saved as `rclone_pull_media.bat` and `qb_cleanup_ratio.ps1` in the `scripts` directory.

## 5. Final Commit Point

All work related to the implementation of the first-time setup wizards will be committed under a single, descriptive feature commit.

- **Commit Message:** `feat: Implement first-time setup wizards for Rclone and qB Cleanup`