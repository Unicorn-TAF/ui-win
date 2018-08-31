PM> Get-Project Tests | Install-Package ReportPortal.NUnit -Version 3.0.0-beta-21 -Pre
D:\Dev\dotNet\NUnit.Console-3.5.0 to path
ReportPortal.addins file -> ../Projects/UniversalFramework/Tests/bin/Release/ReportPortal.NUnitExtension.dll (should be relative to nunit console exepath, without full path with disk and back slashes)

nunit3-console Tests.dll --where "class=~UnitTests"

RestSharp for 4.5.2 should be at least 105.2.1