# LeadDesk

ASP.NET Core (.NET 10) Razor Pages app with full CRUD for sales leads.
No database: data lives in an in-memory store (`Services/LeadStore.cs`) seeded with sample leads.
It resets whenever the app restarts or scales out.

## Run locally
    dotnet run
Open the URL printed in the console.

## Build the deployment zip
Needs the .NET 10 SDK.

    ./build-package.ps1      (Windows PowerShell)
    ./build-package.sh       (macOS / Linux)

This creates `deploy.zip` (the published output).

## Deploy to Azure App Service (Linux)
    az login
    az group create -n leaddesk-rg -l centralindia
    az appservice plan create -n leaddesk-plan -g leaddesk-rg --sku B1 --is-linux
    az webapp create -n <unique-app-name> -g leaddesk-rg -p leaddesk-plan --runtime "DOTNETCORE:10.0"
    az webapp deploy -g leaddesk-rg -n <unique-app-name> --src-path deploy.zip --type zip

Browse to https://<unique-app-name>.azurewebsites.net

Portal alternative: create a Web App (Publish: Code, Runtime stack: .NET 10), then
Deployment Center / Advanced Tools (Kudu) > Zip Push Deploy and upload `deploy.zip`.

Notes
- Health probe endpoint: `/healthz`
- Keep to a single instance: each instance has its own memory, so scaling out shows different data per instance.
- On the Free (F1) tier the app sleeps when idle and loses its data; use B1 or higher with Always On for demos.
- To swap in a real database later, implement `ILeadStore` and change the registration in `Program.cs`.
