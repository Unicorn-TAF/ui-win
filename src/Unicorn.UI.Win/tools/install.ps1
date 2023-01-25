param($installPath, $toolsPath, $package, $project)

$project.Object.References | Where-Object { $_.Name -eq "Interop.UIAutomationClient" } |  ForEach-Object { $_.EmbedInteropTypes = $false }