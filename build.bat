@echo off
rem Prérequis : .NET 8 SDK + Inno Setup 6 (iscc dans le PATH ou chemin ci-dessous)
dotnet publish Launcher\Valydium.Launcher.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish || exit /b 1
set ISCC="%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe"
%ISCC% installer\valydium.iss || exit /b 1
echo.
echo OK : dist\VALYDIUM_Installer.exe
