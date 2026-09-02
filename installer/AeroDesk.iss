#define MyAppName "AeroDesk"
#define MyAppVersion "0.2.0"
#define MyAppPublisher "carjam120443-netizen"
#define MyAppExeName "AeroDesk.exe"

[Setup]
AppId={{AERODESK-7F8A-4E18-B6D3-0B6C6D0D2026}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={localappdata}\AeroDesk
DisableProgramGroupPage=yes
OutputDir=output
OutputBaseFilename=AeroDesk-Setup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64
WizardStyle=modern

[Types]
Name: "normal"; Description: "Normal installation"
Name: "portable"; Description: "Portable (no shortcuts)"

[Files]
Source: "..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\AeroDesk"; Filename: "{app}\{#MyAppExeName}"; Tasks: normalicon
Name: "{autodesktop}\AeroDesk"; Filename: "{app}\{#MyAppExeName}"; Tasks: normalicon

[Tasks]
Name: "normalicon"; Description: "Create Start Menu and desktop shortcuts"; Flags: unchecked

[Code]
procedure InitializeWizard;
begin
  WizardForm.TypesCombo.Text := 'Normal installation';
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;
  if CurPageID = wpSelectDir then
  begin
    if WizardForm.TypesCombo.ItemIndex = 1 then
      WizardForm.DirEdit.Text := ExpandConstant('{userappdata}\AeroDesk-Portable');
  end;
end;
