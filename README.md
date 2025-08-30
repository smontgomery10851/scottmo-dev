# ScottMoDeluxe (.NET 8, Identity, Web API)
Minimal ASP.NET Core 8 app with Razor Pages, Identity (using existing AspNetUsers schema), and a sample Web API.

## Run locally
1. Install .NET 8 SDK.
2. Copy your connection string to `dotnet user-secrets` (Windows):
   ```ps1
   cd src/ScottMo.Web
   dotnet user-secrets set "ConnectionStrings:Default" "Server=YOUR_SQL_HOST,1433;Database=aspnetusers;User Id=YOUR_USER;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
   ```
3. `dotnet run` then open the URL shown.

## Publish (self-contained for GoDaddy Windows/Plesk)
```ps1
dotnet publish src/ScottMo.Web/ScottMo.Web.csproj -c Release -o publish -r win-x86 --self-contained true /p:PublishReadyToRun=true
```
Upload the `publish/` folder contents to `/httpdocs/` via FTPS.

## GitHub Actions (FTPS)
Create repo secrets:
- `GD_FTPS_SERVER` (e.g. your Plesk FTPS host)
- `GD_FTPS_USER`
- `GD_FTPS_PASSWORD`

Push to `main` to deploy.
