# Azure Deployment Instructions for Sports Card Store

## Step 1: Create Azure Resources

**📝 IMPORTANT**: Before running the script, edit `azure-setup-commands.ps1` and change `$adminPassword` to a secure password of your choice.

```powershell
# Run this in PowerShell (as Administrator)
.\azure-setup-commands.ps1
```

## Step 2: Configure Connection Strings Securely

**🔒 SECURITY IMPORTANT**: Never add connection strings to appsettings.json in a public repository.

### For Local Development (Use dotnet user-secrets):

```powershell
# Initialize user secrets for the API project
dotnet user-secrets init --project src/SportsCardStore.API

# Store the connection string securely (replace with your actual password)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=tcp:sportscard-sql-server.database.windows.net,1433;Initial Catalog=SportscardDb;User ID=sqladmin;Password=YourSecurePassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" --project src/SportsCardStore.API
```

### For Production (Azure App Service Configuration):

Connection strings will be configured in Azure App Service Configuration during deployment. The application automatically reads from Azure App Service Configuration in production environments.

## Step 3: Add Your IP to SQL Firewall

Get your current IP address and add it to the SQL Server firewall:

```powershell
# Get your IP
$myIP = (Invoke-RestMethod -Uri "https://api.ipify.org").Trim()
Write-Host "Your IP: $myIP"

# Add firewall rule
az sql server firewall-rule create `
    --resource-group "sports-card-store-rg" `
    --server "sportscard-sql-server" `
    --name "MyCurrentIP" `
    --start-ip-address $myIP `
    --end-ip-address $myIP
```

## Step 4: Create and Run Database Migrations

```powershell
# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate --project src/SportsCardStore.Infrastructure --startup-project src/SportsCardStore.API

# Update database
dotnet ef database update --project src/SportsCardStore.Infrastructure --startup-project src/SportsCardStore.API
```

## Step 5: Deploy to Azure

```powershell
# Build and publish
dotnet publish src/SportsCardStore.API -c Release -o ./publish

# Deploy to Azure (from publish folder)
az webapp deployment source config-zip `
    --resource-group "sports-card-store-rg" `
    --name "sportscard-api" `
    --src "./publish.zip"
```

## Your Azure Resources

After successful creation, you'll have:

- **Resource Group**: `sports-card-store-rg`
- **SQL Server**: `sportscard-sql-server.database.windows.net`
- **Database**: `SportscardDb` (Basic tier)
- **App Service**: `sportscard-api.azurewebsites.net`
- **App Service Plan**: `sportscard-app-service-plan` (Basic B1)

## Connection String Template

```
Server=tcp:sportscard-sql-server.database.windows.net,1433;Initial Catalog=SportscardDb;User ID=sqladmin;Password=[YOUR_PASSWORD];MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

Replace `[YOUR_PASSWORD]` with the password you set in the script.
