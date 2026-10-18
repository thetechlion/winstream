; Inno Setup script for WinStream
; Put this file in the same folder as WinStream.csproj, publish first, then compile it.

#define MyAppName "WinStream"
#define MyAppVersion "1.0.0"
#define MyAppExe "WinStream.exe"
#define PublishDir "bin\Release\net8.0-windows\win-x64\publish"

[Setup]
AppId={{B7E4C2A1-5D3F-4E8A-9C61-2F0A7D9E4B35}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=installer
OutputBaseFilename=WinStream-Setup
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
UninstallDisplayIcon={app}\{#MyAppExe}
; Uncomment and point at your icon if you have one:
; SetupIconFile=WinStream.ico

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked

[Files]
Source: "{#PublishDir}\{#MyAppExe}"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExe}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExe}"; Description: "Launch {#MyAppName}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Removes saved sign-ins on uninstall
Type: filesandordirs; Name: "{localappdata}\WinStream"
