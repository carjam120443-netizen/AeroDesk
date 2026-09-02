param(
    [ValidateSet('Normal','Portable')]
    [string]$InstallMode = 'Normal'
)

$ErrorActionPreference = 'Stop'
$AppName = 'AeroDesk'
$Source = Join-Path $PSScriptRoot 'AeroDesk'

if (-not (Test-Path $Source)) {
    throw "AeroDesk publish output was not found at $Source"
}

if ($InstallMode -eq 'Portable') {
    $Target = Join-Path $PSScriptRoot 'AeroDesk-Portable'
} else {
    $Target = Join-Path $env:LOCALAPPDATA 'AeroDesk'
}

New-Item -ItemType Directory -Force -Path $Target | Out-Null
Copy-Item -Path (Join-Path $Source '*') -Destination $Target -Recurse -Force

if ($InstallMode -eq 'Normal') {
    $StartMenu = Join-Path $env:APPDATA 'Microsoft\Windows\Start Menu\Programs'
    New-Item -ItemType Directory -Force -Path $StartMenu | Out-Null
    $ShortcutPath = Join-Path $StartMenu 'AeroDesk.lnk'
    $ExePath = Join-Path $Target 'AeroDesk.exe'
    $Shell = New-Object -ComObject WScript.Shell
    $Shortcut = $Shell.CreateShortcut($ShortcutPath)
    $Shortcut.TargetPath = $ExePath
    $Shortcut.WorkingDirectory = $Target
    $Shortcut.Description = 'AeroDesk desktop widgets and launcher'
    $Shortcut.Save()
}

Write-Host "AeroDesk installed in $InstallMode mode at $Target"
