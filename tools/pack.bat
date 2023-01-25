@echo off
set PKG=Unicorn.UI.Win
set PROJ_PATH=%cd%\..\src\%PKG%\%PKG%
set OUT_DIR=%1

dotnet pack %PROJ_PATH%.csproj -o %OUT_DIR% -c Release -p:NuspecFile=%PROJ_PATH%.nuspec