# Azure Deployment Phase 9 - Configuration Summary

This document outlines the changes made for Phase 9 deployment to Azure.

## Files Modified

### 1. `src/SportsCardStore.API/Program.cs`
- **CORS Enhancement**: Added support for production frontend URL via `FrontendUrl` configuration
- **Database Initialization**: Now runs EF migrations in production (not just development)
- **Production Ready**: Handles both SQLite (dev) and SQL Server (production) automatically

### 2. `azure-pipelines.yml`
- **EF Migration Step**: Added automatic database migration before deployment
- **Tool Installation**: Installs `dotnet-ef` tool in the pipeline
- **Connection String**: Uses pipeline variable `$(AzureConnectionString)` for migrations

### 3. `.github/workflows/deploy-frontend.yml` (New)
- **GitHub Actions**: Automated deployment of React frontend to Azure Static Web Apps
- **Build Configuration**: Sets `VITE_API_BASE_URL` to production API URL during build
- **Triggers**: Runs on push to main when frontend files change

### 4. `src/SportsCardStore.API/appsettings.json`
- **Placeholders**: Added empty placeholders for Azure configuration (filled by App Service)
- **Blob Storage**: Configuration structure for Azure Blob Storage
- **Frontend URL**: Placeholder for production frontend URL

### 5. `deploy-azure-phase9.ps1` (New)
- **Complete Setup**: PowerShell script with all required Azure CLI commands
- **App Service Config**: Sets all required application settings and connection strings
- **Static Web App**: Creates the Static Web App and outputs deployment token
- **Documentation**: Provides next steps and URLs for testing

## Azure Resources Configuration

### App Service Settings
```bash
# Connection Strings
DefaultConnection=Server=sportscard-sql-server.database.windows.net;Database=SportscardDb;...

# Application Settings
AzureBlobStorage:ConnectionString=DefaultEndpointsProtocol=https;AccountName=sportscardstore;...
AzureBlobStorage:ContainerName=card-images
ASPNETCORE_ENVIRONMENT=Production
FrontendUrl=https://sportscard-web.azurestaticapps.net
```

### Static Web App
- **Name**: sportscard-web
- **Location**: eastus2
- **SKU**: Free
- **Source**: GitHub repository
- **Build Location**: `/src/SportsCardStore.Web`
- **Output**: `dist/`

## Deployment Process

### Backend (API)
1. **Azure Pipeline Trigger**: Push to main branch
2. **Build & Test**: Compile and run unit tests
3. **Database Migration**: Run EF migrations against Azure SQL
4. **Deploy**: Deploy to Azure App Service

### Frontend (React)
1. **GitHub Actions Trigger**: Push to main branch (frontend files changed)
2. **Build**: Install dependencies and build with production API URL
3. **Deploy**: Deploy to Azure Static Web Apps

## Environment Variables Required

### Azure Pipeline Variables
- `AzureServiceConnection`: Azure service connection name
- `AzureConnectionString`: Azure SQL connection string

### GitHub Secrets
- `AZURE_STATIC_WEB_APPS_API_TOKEN`: Deployment token from Static Web App

## Production URLs
- **API**: https://sportscard-api.azurewebsites.net
- **Frontend**: https://sportscard-web.azurestaticapps.net
- **Database**: sportscard-sql-server.database.windows.net/SportscardDb
- **Storage**: sportscardstore (container: card-images)

## Security Features
- ✅ No credentials hardcoded in source code
- ✅ All secrets stored in Azure App Service Configuration
- ✅ GitHub Secrets for deployment tokens
- ✅ CORS configured for production origins only
- ✅ HTTPS enforced in production

## Testing Deployment
1. Run `deploy-azure-phase9.ps1` to configure Azure resources
2. Add deployment token to GitHub Secrets
3. Push changes to main branch
4. Monitor Azure Pipeline and GitHub Actions
5. Test both API and frontend endpoints