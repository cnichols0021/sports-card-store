# ===============================================
# Azure Deployment Commands for SportsCardStore Phase 9
# ===============================================
# Run these commands in PowerShell with Azure CLI installed.
# Make sure you're logged in: az login
# Set the correct subscription: az account set --subscription "Your-Subscription-Name"
#
# BEFORE RUNNING:
# 1. Replace YourStrongPassword123! with your actual SQL admin password
# 2. Replace YOUR_GITHUB_USERNAME with your GitHub username (cnichols0021)

# Variables
$resourceGroup = "sports-card-store-rg"
$appServiceName = "sportscard-api"
$staticWebAppName = "sportscard-web"
$sqlServer = "sportscard-sql-server.database.windows.net"
$sqlDatabase = "SportscardDb"
$storageAccount = "sportscardstore"
$location = "eastus2"
$sqlPassword = "SportsC@rds@pr1l2026"  # <-- UPDATE THIS

# ===============================================
# Task 1: Configure Azure App Service Application Settings
# ===============================================
Write-Host "Configuring Azure App Service application settings..." -ForegroundColor Yellow

# Set SQL Server connection string
az webapp config connection-string set `
  --resource-group $resourceGroup `
  --name $appServiceName `
  --connection-string-type SQLServer `
  --settings DefaultConnection="Server=tcp:$sqlServer,1433;Initial Catalog=$sqlDatabase;User ID=sqladmin;Password=$sqlPassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

# Get storage account connection string dynamically
$storageConnectionString = az storage account show-connection-string `
  --resource-group $resourceGroup `
  --name $storageAccount `
  --query connectionString `
  --output tsv

# Set app settings
az webapp config appsettings set `
  --resource-group $resourceGroup `
  --name $appServiceName `
  --settings `
    "AzureBlobStorage:ConnectionString=$storageConnectionString" `
    "AzureBlobStorage:ContainerName=card-images" `
    "ASPNETCORE_ENVIRONMENT=Production"

Write-Host "App Service configuration complete" -ForegroundColor Green

# ===============================================
# Task 3: Create Azure Static Web App
# ===============================================
# Note: --source is intentionally omitted. Deployment is handled by
# .github/workflows/deploy-frontend.yml using AZURE_STATIC_WEB_APPS_API_TOKEN.
# Adding --source here would create a second conflicting GitHub Actions workflow.
Write-Host "Creating Azure Static Web App..." -ForegroundColor Yellow

$staticWebAppJson = az staticwebapp create `
  --name $staticWebAppName `
  --resource-group $resourceGroup `
  --location $location `
  --sku Free `
  --query "{name:name, defaultHostname:defaultHostname}" `
  --output json

$staticWebApp = $staticWebAppJson | ConvertFrom-Json
$staticWebAppUrl = "https://$($staticWebApp.defaultHostname)"
Write-Host "Static Web App created: $staticWebAppUrl" -ForegroundColor Green

# Get deployment token for GitHub Secrets
$deploymentToken = az staticwebapp secrets list `
  --name $staticWebAppName `
  --resource-group $resourceGroup `
  --query "properties.apiKey" `
  --output tsv

Write-Host "`n DEPLOYMENT TOKEN" -ForegroundColor Magenta
Write-Host "Add this as AZURE_STATIC_WEB_APPS_API_TOKEN in GitHub Secrets:" -ForegroundColor Yellow
Write-Host "  Repo -> Settings -> Secrets and variables -> Actions -> New repository secret" -ForegroundColor Gray
Write-Host $deploymentToken -ForegroundColor Cyan

# ===============================================
# Task 2: Set Frontend URL on App Service (enables production CORS)
# ===============================================
Write-Host "`nAdding frontend URL to App Service CORS config..." -ForegroundColor Yellow

az webapp config appsettings set `
  --resource-group $resourceGroup `
  --name $appServiceName `
  --settings "FrontendUrl=$staticWebAppUrl"

Write-Host "Frontend URL configured" -ForegroundColor Green

# ===============================================
# Summary
# ===============================================
Write-Host "`n===============================================" -ForegroundColor White
Write-Host "Azure deployment configuration complete!" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Add the deployment token to GitHub Secrets (printed above)" -ForegroundColor White
Write-Host "   Repo -> Settings -> Secrets and variables -> Actions" -ForegroundColor Gray
Write-Host "   Secret name: AZURE_STATIC_WEB_APPS_API_TOKEN" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Add these variables in Azure Pipelines:" -ForegroundColor White
Write-Host "   AzureServiceConnection  -> your Azure DevOps service connection name" -ForegroundColor Gray
Write-Host "   AzureConnectionString   -> Server=tcp:$sqlServer,1433;Initial Catalog=$sqlDatabase;..." -ForegroundColor Gray
Write-Host ""
Write-Host "3. Production URLs:" -ForegroundColor White
Write-Host "   API:      https://$appServiceName.azurewebsites.net" -ForegroundColor Gray
Write-Host "   Frontend: $staticWebAppUrl" -ForegroundColor Gray
Write-Host ""
Write-Host "4. Push to main to trigger both pipelines" -ForegroundColor White
