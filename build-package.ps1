# Builds a ready-to-upload deploy.zip (Windows PowerShell). Requires the .NET 10 SDK.
$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot
if (Test-Path ./publish) { Remove-Item ./publish -Recurse -Force }
if (Test-Path ./deploy.zip) { Remove-Item ./deploy.zip -Force }
dotnet publish ./LeadDesk.csproj -c Release -o ./publish
Compress-Archive -Path ./publish/* -DestinationPath ./deploy.zip
Write-Host "Created deploy.zip"
