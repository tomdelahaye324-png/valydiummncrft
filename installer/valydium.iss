; Inno Setup 6 : même technologie que l'exemple Paladium (Inno Setup Setup Data 6.1.0)
#define AppName "VALYDIUM Launcher"
#define AppExe  "VALYDIUM Launcher.exe"
[Setup]
AppId={{7B1C2F54-3D1E-4B1A-9C55-5A7E2D0F1A11}
AppName={#AppName}
AppVersion=1.0.0
AppPublisher=VALYDIUM
DefaultDirName={autopf}\VALYDIUM Launcher
DefaultGroupName=VALYDIUM
UninstallDisplayName={#AppName}
UninstallDisplayIcon={app}\{#AppExe}
OutputDir=..\dist
OutputBaseFilename=VALYDIUM_Installer
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
ArchitecturesInstallIn64BitMode=x64compatible
[Languages]
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
[Tasks]
Name: "desktopicon"; Description: "Créer un raccourci sur le Bureau"; GroupDescription: "Raccourcis :"
[Files]
Source: "..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
[Icons]
Name: "{autodesktop}\VALYDIUM"; Filename: "{app}\{#AppExe}"; Tasks: desktopicon
Name: "{group}\VALYDIUM"; Filename: "{app}\{#AppExe}"
Name: "{group}\Désinstaller VALYDIUM"; Filename: "{uninstallexe}"
[Run]
Filename: "{app}\{#AppExe}"; Description: "Lancer VALYDIUM"; Flags: nowait postinstall skipifsilent
