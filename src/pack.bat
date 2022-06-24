@echo off
set TAF_ROOT=%1
set OUT_DIR=%2

for %%p in (Unicorn.Taf.Api Unicorn.Taf.Core Unicorn.Backend) do (dotnet pack %TAF_ROOT%\%%p\%%p.csproj -o %OUT_DIR% -c Release --no-build -p:NuspecFile=%TAF_ROOT%\%%p\%%p.nuspec)
for %%p in (Core Win Web Mobile) do (dotnet pack %TAF_ROOT%\Unicorn.UI\%%p\Unicorn.UI.%%p.csproj -o %OUT_DIR% -c Release --no-build -p:NuspecFile=%TAF_ROOT%\Unicorn.UI\%%p\Unicorn.UI.%%p.nuspec)