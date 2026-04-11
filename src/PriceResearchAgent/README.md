# Price Research Agent

A console application that researches sports card pricing using multiple data sources to provide reliable price recommendations based on real market data.

## Overview

The Price Research Agent uses a pluggable architecture with multiple pricing sources to find the best available pricing data for any sports card. It tries sources in priority order and returns comprehensive pricing analysis with confidence ratings.

## Features

- **Multi-Source Architecture**: Pluggable `IPricingSource` interface allows easy addition of new data sources
- **eBay Finding API Integration**: Real-time completed sales data from eBay marketplace
- **Intelligent Pricing Logic**: Suggests low, high, and optimal listing prices based on market data
- **Confidence Ratings**: High/Medium/Low confidence based on number of sales analyzed
- **Comprehensive Logging**: Structured logging throughout the research process
- **Flexible Configuration**: Support for user secrets and environment variables

## Current Data Sources

### 1. eBay Finding API (Primary)

- Real completed sales data from eBay marketplace using findCompletedItems operation
- Filters for US listings, sold items only, and relevant matches
- Analyzes price distribution and actual sale dates
- Requires eBay Developer App ID (no authentication token needed)

### Future Sources

- **Card Ladder API** (when available): Industry-standard pricing data
- **Card Ladder Web Scrape**: Fallback for Card Ladder data without API access

## Setup

### 1. eBay Developer Account

1. Create account at [developer.ebay.com](https://developer.ebay.com)
2. Create a new application to get your App ID
3. The Finding API requires only an App ID (no OAuth token needed)

### 2. Configure App ID

**Option A: User Secrets (Recommended)**

```powershell
cd src/PriceResearchAgent
dotnet user-secrets set "eBay:AppId" "your-app-id-here"
```

**Option B: Environment Variable**

```powershell
$env:EBAY_APP_ID = "your-app-id-here"
```

### 3. Build the Application

```powershell
cd src/PriceResearchAgent
dotnet build
```

## Usage

### Basic Syntax

```powershell
dotnet run -- <player> <year> <brand> <set> [parallel] [grading_company] [grade]
```

### Examples

**Raw Card**

```powershell
dotnet run -- \"Ronald Acuna Jr\" 2018 \"Topps\" \"Chrome\" \"\" \"Raw\" \"\"
```

**Graded Card**

```powershell
dotnet run -- \"Mike Trout\" 2011 \"Topps\" \"Update\" \"\" \"PSA\" 10
```

**Parallel Card**

```powershell
dotnet run -- \"Connor Bedard\" 2023 \"Upper Deck\" \"Series 1\" \"Gold\" \"BGS\" 9.5
```

### Sample Output

```
🎯 Price Research Results
═══════════════════════════════════════════════════
Card: 2018 Topps Chrome Ronald Acuna Jr Raw/Ungraded
Data Source: eBay Browse API
Sales Analyzed: 23
Confidence: High

💰 Suggested Pricing:
  Low Price:     $8.50
  High Price:    $24.99
  Listing Price: $16.75 (recommended starting point)

📊 Recent Sales (up to 10):
  $12.50 - eBay - 2018 Topps Chrome Ronald Acuna Jr. Rookie Card #193
  $15.00 - eBay - Ronald Acuna Jr 2018 Topps Chrome RC Near Mint
  $18.99 - eBay - 2018 Topps Chrome #193 Ronald Acuna Jr Rookie PSA...

🔗 Source URL: https://www.ebay.com/sch/i.html?_nkw=...

Generated: 2026-04-11 15:30:45 UTC
```

## Architecture

### IPricingSource Interface

```csharp
public interface IPricingSource
{
    string SourceName { get; }
    Task<PricingResult?> GetPricingAsync(CardPricingRequest request);
}
```

### Source Priority Order

1. **eBay Browse API** - Currently implemented
2. **Future**: Card Ladder API (when available)
3. **Future**: Card Ladder Web Scraper (fallback)

### Data Models

- `CardPricingRequest`: Input card details
- `PricingResult`: Raw pricing data from a source
- `PriceResearchResponse`: Final formatted response with suggestions
- `RecentSale`: Individual sale transaction data

## Configuration

### Required Settings

- **eBay Access Token**: Required for eBay API access

### Optional Settings

- **Logging Level**: Configured via `appsettings.json` or environment variables
- **API Timeouts**: HttpClient timeout settings

## Error Handling

The agent is designed to be resilient:

- **Source Failures**: If one source fails, tries the next in priority order
- **Invalid Data**: Filters out obviously incorrect prices and listings
- **Network Issues**: Graceful handling of HTTP timeouts and connection problems
- **Configuration Issues**: Clear error messages for missing or invalid configuration

## Integration with Sports Card Store

### Inventory Management

- Research pricing before adding new inventory
- Validate listing prices against current market data
- Identify undervalued or overpriced items

### Marketplace Integration

- Generate competitive listing prices for eBay, COMC, etc.
- Research pricing for bulk lots and collections
- Monitor market trends for specific cards or sets

## Development Notes

- Built using Clean Architecture patterns
- Dependency injection for testability and modularity
- Comprehensive logging via `Microsoft.Extensions.Logging`
- Uses `System.Text.Json` for all JSON operations
- Follows the same project structure as other Sports Card Store agents

## Future Enhancements

- **Card Ladder API Integration**: Primary data source when API access is available
- **COMC Integration**: Additional marketplace data
- **Historical Trends**: Track pricing changes over time
- **Bulk Research**: Process multiple cards in batch operations
- **Web Dashboard**: Visual pricing research interface
- **Caching Layer**: Cache results to reduce API calls and improve performance

## Troubleshooting

### \"No pricing data found\"

- Verify eBay access token configuration
- Check that card details are accurate and specific enough
- Try variations of player name or set name
- Ensure card exists and has recent sales activity

### \"eBay API error\"

- Verify access token is valid and not expired
- Check eBay Developer Dashboard for API usage limits
- Ensure network connectivity to eBay APIs

### Command line parsing errors

- Use quotes around parameters with spaces
- Follow the exact argument order: player, year, brand, set, parallel, grading company, grade
- Leave empty strings (\"\") for optional parameters you want to skip
