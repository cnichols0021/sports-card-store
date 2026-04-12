# ===============================================
# Azure Deployment Commands for SportsCardStore Phase 9
# ===============================================
# Run these commands in PowerShell with Azure CLI installed.
# Make sure you're logged in: az login
# Set the correct subscription: az account set --subscription "Your-Subscription-Name"

# Variables - Update these with your actual values
$resourceGroup = "sports-card-store-rg"
$appServiceName = "sportscard-api"
$staticWebAppName = "sportscard-web"
$sqlServer = "sportscard-sql-server.database.windows.net"
$sqlDatabase = "SportscardDb"
$storageAccount = "sportscardstore"
$location = "eastus2"

# ===============================================
# Task 1: Configure Azure App Service Application Settings
# ===============================================
Write-Host "Configuring Azure App Service application settings..." -ForegroundColor Yellow

# Set SQL Server connection string (update with your actual SQL credentials)
az webapp config connection-string set `
  --resource-group $resourceGroup `
  --name $appServiceName `
  --connection-string-type SQLServer `
  --settings DefaultConnection="Server=$sqlServer;Database=$sqlDatabase;User Id=sportscard-admin;Password=YourStrongPassword123!;TrustServerCertificate=true;MultipleActiveResultSets=true"

# Get storage account connection string
$storageConnectionString = az storage account show-connection-string `
  --resource-group $resourceGroup `
  --name $storageAccount `
  --query connectionString `
  --output tsv

# Set application settings
az webapp config appsettings set `
  --resource-group $resourceGroup `
  --name $appServiceName `
  --settings `
    "AzureBlobStorage:ConnectionString=$storageConnectionString" `
    "AzureBlobStorage:ContainerName=card-images" `
    "ASPNETCORE_ENVIRONMENT=Production"

Write-Host "✅ App Service configuration complete" -ForegroundColor Green

# ===============================================
# Task 3: Create Azure Static Web App
# ===============================================
Write-Host "Creating Azure Static Web App..." -ForegroundColor Yellow

$staticWebApp = az staticwebapp create `
  --name $staticWebAppName `
  --resource-group $resourceGroup `
  --location $location `
  --sku Free `
  --source https://github.com/YOUR_GITHUB_USERNAME/SportsCardStore `
  --branch main `
  --app-location "/src/SportsCardStore.Web" `
  --output-location "dist" `
  --query "{name:name, defaultHostname:defaultHostname}" `
  --output json | ConvertFrom-Json

$staticWebAppUrl = "https://$($staticWebApp.defaultHostname)"
Write-Host "Static Web App created: $staticWebAppUrl" -ForegroundColor Green

# Get deployment token
$deploymentToken = az staticwebapp secrets list `
  --name $staticWebAppName `
  --resource-group $resourceGroup `
  --query "properties.apiKey" `
  --output tsv

Write-Host "🔐 DEPLOYMENT TOKEN (save this as AZURE_STATIC_WEB_APPS_API_TOKEN in GitHub Secrets):" -ForegroundColor Magenta
Write-Host $deploymentToken -ForegroundColor Cyan

# ===============================================
# Task 2: Update App Service with Frontend URL
# ===============================================
Write-Host "Adding frontend URL to App Service configuration..." -ForegroundColor Yellow

az webapp config appsettings set `
  --resource-group $resourceGroup `
  --name $appServiceName `
  --settings "FrontendUrl=$staticWebAppUrl"

Write-Host "✅ Frontend URL configured in App Service" -ForegroundColor Green

# ===============================================
# Summary
# ===============================================
Write-Host "`n===============================================" -ForegroundColor White
Write-Host "🎉 Azure deployment configuration complete!" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor White
Write-Host "📋 Next Steps:" -ForegroundColor Yellow
Write-Host "1. Add the deployment token to GitHub Secrets:" -ForegroundColor White
Write-Host "   • Go to your GitHub repo → Settings → Secrets and variables → Actions" -ForegroundColor Gray
Write-Host "   • Add secret: AZURE_STATIC_WEB_APPS_API_TOKEN" -ForegroundColor Gray
Write-Host "   • Value: $deploymentToken" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Update your Azure Pipeline variables:" -ForegroundColor White
Write-Host "   • AzureConnectionString: Server=$sqlServer;Database=$sqlDatabase;User Id=sportscard-admin;Password=YourStrongPassword123!;..." -ForegroundColor Gray
Write-Host ""
Write-Host "3. Your production URLs:" -ForegroundColor White
Write-Host "   • API: https://$appServiceName.azurewebsites.net" -ForegroundColor Gray
Write-Host "   • Frontend: $staticWebAppUrl" -ForegroundColor Gray
Write-Host ""
Write-Host "4. Test the deployment by pushing to main branch" -ForegroundColor White