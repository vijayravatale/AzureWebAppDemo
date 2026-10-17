#!/usr/bin/env bash
# Builds a ready-to-upload deploy.zip (macOS/Linux). Requires the .NET 10 SDK and zip.
set -e
cd "$(dirname "$0")"
rm -rf publish deploy.zip
dotnet publish ./LeadDesk.csproj -c Release -o ./publish
(cd publish && zip -r ../deploy.zip .)
echo "Created deploy.zip"
