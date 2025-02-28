# VaccineSchedule
VaccineSchedule is a Web API built on ASP.NET Core 8 designed to help parents manage their children's Vaccine Schedules.


create Window services

1. open CMD(Admin) and access to .csproj or .slm file
Ex: E:\Backup\SWP391\VaccineScheduleTracking_Test
or: E:\Backup\SWP391\VaccineScheduleTracking_Test\VaccineScheduleTracking.API

2. Run in Powershell(Admin)
New-Service -Name "MyScheduledTaskService" -BinaryPathName "E:\Backup\SWP391\VaccineScheduleTracking_Test\VaccineScheduleTracking.API\bin\Release\net8.0\VaccineScheduleTracking.API_Test.exe" -DisplayName "Scheduled Task Service" -StartupType Automatic

3. Start Service
Start-Service -Name "MyScheduledTaskService"

   View Status
Get-Service -Name "MyScheduledTaskService"