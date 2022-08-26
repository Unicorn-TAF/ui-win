@echo off
set TAF_ROOT=%cd%\..\src
set OUT_DIR=%1

for %%p in (Unicorn.Taf.Api Unicorn.Taf.Core Unicorn.Backend) do (dotnet pack %TAF_ROOT%\%%p\%%p.csproj -o %OUT_DIR% -c Release --no-build -p:NuspecFile=%TAF_ROOT%\%%p\%%p.nuspec)
for %%p in (Core Win Web Mobile) do (dotnet pack %TAF_ROOT%\Unicorn.UI\%%p\Unicorn.UI.%%p.csproj -o %OUT_DIR% -c Release --no-build -p:NuspecFile=%TAF_ROOT%\Unicorn.UI\%%p\Unicorn.UI.%%p.nuspec)