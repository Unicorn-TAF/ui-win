@echo off
set PKG=Unicorn.UI.Win
set VERSION=%1
set OUT_DIR=%2

dotnet pack src\%PKG%\%PKG%.csproj -o %OUT_DIR% -c Release -p:NuspecFile=%PKG%.nuspec -p:Version=%VERSION%