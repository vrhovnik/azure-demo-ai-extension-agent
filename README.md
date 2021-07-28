# Demo application to demonstrate usage for virtual machines using Application Insights Agent (.NET framework)

Demo app to demonstrate usage for virtual machines using Application Insights Agent ([.NET framework](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/intro)) to demonstrate functionalities like [**auto instrumentation**](https://docs.microsoft.com/en-us/azure/azure-monitor/app/azure-vm-vmss-apps#enable-application-insights) approach, [advanced SQL tracking to get full SQL query](https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-dependencies#advanced-sql-tracking-to-get-full-sql-query), etc.

## AI Agent diagnostics

If agent doesn't collect data or there are some challenges with getting SQL data, then you can try a few steps to mitigate the challenge:

1. **make sure**, that status monitor is sending data to Azure (check Live Stream in Application Insights) or run the query in logs section in Application Insights (``dependencies | where timestamp > ago(1d) | summarize count() by cloud_RoleInstance, sdkVersion``) - if the data prefix is **rddf**, instead of **rddp**, advanded logs (like SQL commands) is not sending

![AI Agent Logs Information](https://webeudatastorage.blob.core.windows.net/web/ai-agent-logs-info.png)

2. try to **correct permissions** to have rights to write to temp data and to app data local

**Powershell:**

``icacls $env:WINDIR\Temp /t /c /grant IIS_IUSRS:'(OI)(CI)M'``

``icacls $env:WINDIR\System32\config\systemprofile\AppData\Local /t /c /grant IIS_IUSRS:'(OI)(CI)M'``

3. do IIS restart (iisreset via Powershell) (or [stop / start from IIS manager](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-R2-and-2012/jj635851(v=ws.11)))

If that doesn't work, make sure you you have rigth pre-prequisite in place:

``Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force``

``Install-Module -Name Az.ApplicationMonitor -AllowPrerelease -AcceptLicense``

If you receive an error, that module doesn't exists, install nuget and powershellget module:

``Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force``

``Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force``

``Set-PSRepository -Name "PSGallery" -InstallationPolicy Trusted``

``Install-Module -Name PowerShellGet -Force``

Before you continue, **restart PowerShell session** (run the PowerShell as admin again).

Enable **JUST** the instrumentation engine:

``Enable-InstrumentationEngine``

And do **IIS reset** (as defined in step 3 above).

If you do that, you should see the results in the Application Insights in Azure Portal blade via end-to-end transaction details as defined below:

![END to END transaction details with full SQL query](https://webeudatastorage.blob.core.windows.net/web/ai-agent-select-command.png)

You can check all of this solutions in more details on [StackOverFlow](https://stackoverflow.com/questions/39410598/application-insights-not-tracking-sql-queries) thread from respective authors.
