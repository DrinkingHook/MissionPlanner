[Setup]
AppName=Mission Planner Custom
AppVersion=1.4.0x
DefaultDirName={pf}\MissionPlannerCustom
DefaultGroupName=Mission Planner
OutputDir=Output
OutputBaseFilename=MissionPlanner_Custom_Setup
Compression=lzma
SolidCompression=true
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Files]
Source: "..\bin\Release\net461\*"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion

[Icons]
Name: "{group}\Mission Planner"; Filename: "{app}\MissionPlanner.exe"
Name: "{group}\卸载 Mission Planner"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\MissionPlanner.exe"; Description: "启动 Mission Planner"; Flags: nowait postinstall skipifsilent
