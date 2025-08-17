# UX Improvement Plan: Adding Non-Technical Explanations

This document outlines the plan to improve the application's user experience by adding non-technical explanations to existing technical labels.

## 1. Design of Contextual Help

To ensure a consistent and intuitive user experience, we will use info icons with tooltips to provide non-technical explanations for technical terms.

- **Visual Cue**: An info icon (e.g., a small "i" in a circle) will be placed next to each technical label.
- **Interaction**: When the user hovers over the info icon, a tooltip will appear with a user-friendly explanation.
- **Styling**: The info icon will be styled to be subtle yet noticeable, and the tooltip will have a consistent look and feel throughout the application.

This approach provides contextual help without cluttering the UI, and it allows users to access additional information only when they need it.
## 2. Content for Explainers

### 2.1. SeedboxConnectionView.xaml

- **Hostname**:
  - **Explainer Text**: "This is the address of your seedbox server (e.g., `server.seedbox.com`). You can usually find this in your seedbox provider's welcome email."

- **Port**:
  - **Explainer Text**: "This is the specific 'door' on your seedbox server that this application needs to connect to. It's usually a number like 22 for SFTP. Check your seedbox provider's documentation if you're unsure."

- **Remote Name**:
  - **Explainer Text**: "This is a nickname you give to your seedbox configuration in Rclone (e.g., `my-seedbox`). It helps you identify this connection later."
### 2.2. TransferJobView.xaml

- **Source Path (Remote)**:
  - **Explainer Text**: "This is the folder on your seedbox where the files you want to transfer are located (e.g., `/home/user/downloads`)."

- **Destination Path (Local)**:
  - **Explainer Text**: "This is the folder on your computer where you want to save the transferred files (e.g., `C:\Users\YourName\Downloads`)."
### 2.3. OpenVPNView.xaml

- **.ovpn configuration file**:
  - **Explainer Text**: "This is a file provided by your VPN service that contains the settings needed to connect to their servers. You can usually download it from your VPN provider's website."
### 2.4. QbittorrentView.xaml

- **Host**:
  - **Explainer Text**: "This is the IP address of the device running qBittorrent. If you're using a VPN, this should be the VPN's IP address. You can use the 'Find my VPN IP' button to help you find it."

- **Port**:
  - **Explainer Text**: "This is the port number that qBittorrent is using for its web interface. The default is usually 8080. You can find this in qBittorrent's Web UI settings."
## 3. Data Presentation Strategy

To improve the user-friendliness of the summary and log views (`CleanupSummaryView.xaml`, `RcloneSummaryView.xaml`, `ActivityDashboardView.xaml`), the following changes will be made:

- **Re-labeling Columns**: Technical column headers will be replaced with more descriptive and user-friendly labels.
- **Filtering Technical Details**: By default, overly technical or verbose information will be hidden from the user. A toggle or "Advanced" view will be provided to allow users to see the full technical details if they choose.
- **Data Formatting**: Data will be formatted for readability (e.g., using human-readable file sizes, relative timestamps).