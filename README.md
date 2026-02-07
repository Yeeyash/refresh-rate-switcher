# RefreshRateSwitcher 🖥️

A lightweight Windows system tray utility to quickly switch between your monitor's supported refresh rates without digging through Windows Display Settings, all via a single-file execution.

<img width="264" height="239" alt="image" src="https://github.com/user-attachments/assets/b2c7790a-d643-42c3-bb3b-64138f8b15aa" />

## Features
* **Tray-based UI**: Access all refresh rates with a simple right-click.
* **Smart Filtering**: Automatically detects and lists only the rates supported by your current resolution.
* **Lightweight**: Written in C# using native Windows APIs (User32.dll).

## How to Use
1. Go to the [Releases](https://github.com/Yeeyash/refresh-rate-switcher/releases/tag/v1.0.0) section.
2. Download the latest `refreshRateSwitcher.zip`.
3. Extract the folder and run `refreshRateSwitcher.exe`.
4. Right-click the monitor icon in your system tray to change your Hz.

## Technical Details
The project is built on **.NET 8.0-Windows**. It utilizes `EnumDisplaySettings` and `ChangeDisplaySettingsEx` to interact with the Windows display driver.

## 🛡️ Security & Windows Defender
When running `refreshRateSwitcher.exe` for the first time, you may see a warning from **Windows SmartScreen** or **Windows Defender** stating that the file is from an "Unknown Publisher."

### Why is this happening?
* **Unsigned Binary**: This is an open-source project. To remove these warnings, a developer must purchase a "Code Signing Certificate" from a Certificate Authority, which costs hundreds of dollars per year.
* **Low Reputation**: Windows Defender flags apps that are new or don't have thousands of users yet.
* **Hardware Interaction**: Because this tool uses `user32.dll` to modify system display settings (Directly interacting with your GPU driver), security software monitors it more closely.

### How to run it:
1. Click **"More info"** on the Windows SmartScreen popup.
2. Click **"Run anyway."**

> **Note:** Since this project is fully open-source, you can review every line of code in `Program.cs` to verify exactly what the application does before running it.

## 🛠️ Current Development Note (Contributors Needed!)
We are currently working on expanding the active display constraints.
* **Current state**: The app only detects the display currently in use and does not work for multiple displays/monitors.
* **Goal**: app should detect all displays, provide available refresh rates for the current resolution.

## System Utilization
- Idle RAM usage (when app is not triggered): ~6mb
- When triggered: ~9mb

If you are a .NET dev and want to help, please check [Issue #1](https://github.com/Yeeyash/refresh-rate-switcher/issues/1#issue-3905179501).
