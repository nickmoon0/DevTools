# DevTools
Simple interface for running tools written in .NET

## How to Run
1. Clone Repo
2. `cd` into `DevTools`
3. Run `dotnet publish -c Release -o publish DevTools.Dashboard/DevTools.Dashboard.csproj`
4. Run `publish/DevToolsDashboard.exe`

## Configuration
The dashboard uses appsettings files to configure environment variables. Create appsettings files with the following patterns: `appsettings.[Environment].json`. The WPF application will load the config into memory at runtime. Appsettings should be in the same directory as the exe file.