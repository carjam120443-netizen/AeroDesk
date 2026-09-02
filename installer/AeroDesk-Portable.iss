#define MyAppName "AeroDesk"
#define MyAppVersion "0.2.0"
#define MyAppPublisher "carjam120443-netizen"
#define MyAppExeName "AeroDesk.exe"

[Setup]
AppId={{AERODESK-PORTABLE-7F8A-4E18-B6D3-0B6C6D0D2026}
AppName={#MyAppName} Portable
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\AeroDesk-Portable
DisableDirPage=no
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
OutputDir=output
OutputBaseFilename=AeroDesk-Portable-Setup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64
WizardStyle=modern
Uninstallable=no

[Files]
Source: "..\publish\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Code]
function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;
end;
