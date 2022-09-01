@echo off
set SRC_ROOT=%cd%\..\src
set PKG=%1
set OUT_DIR=%2

if /I "%PKG:.UI.=%" neq "%PKG%" (
set DIR=%PKG:Unicorn.UI.=%
dotnet pack %SRC_ROOT%\Unicorn.UI\%DIR%\%PKG%.csproj -o %OUT_DIR% -c Release -p:NuspecFile=%SRC_ROOT%\Unicorn.UI\%DIR%\%PKG%.nuspec
) else (
dotnet pack %SRC_ROOT%\%PKG%\%PKG%.csproj -o %OUT_DIR% -c Release -p:NuspecFile=%SRC_ROOT%\%PKG%\%PKG%.nuspec
)