# Card Listing Agent

An AI-powered console application that processes raw sports card descriptions using the Anthropic Claude API to generate structured card listings with titles, descriptions, pricing suggestions, tags, and categorization.

## Overview

The Card Listing Agent accepts natural language descriptions of sports cards and returns structured `CardListing` objects containing:

- **Title**: Professional, concise listing title
- **Description**: Detailed card description with condition and features
- **SuggestedPrice**: AI-generated price recommendation based on market analysis
- **Tags**: Relevant search tags for discoverability
- **Category**: Proper sport categorization (Baseball, Basketball, Football, etc.)

## Requirements

- .NET 10.0
- Anthropic API key
- Internet connection for API calls

## Setup

1. **Get an Anthropic API Key**
   - Sign up at [console.anthropic.com](https://console.anthropic.com)
   - Generate an API key from your dashboard

2. **Configure API Key** (choose one method):

   **Option A: Environment Variable** (Recommended)

   ```powershell
   $env:ANTHROPIC_API_KEY = "your-api-key-here"
   ```

   **Option B: Command Line Argument**

   ```powershell
   dotnet run -- "card description" "your-api-key-here"
   ```

3. **Build the Application**
   ```powershell
   cd src/CardListingAgent
   dotnet build
   ```

## Usage

### Basic Usage

```powershell
dotnet run -- "2023 Topps Ronald Acuna Jr rookie card, mint condition"
```

### With API Key Argument

```powershell
dotnet run -- "1986 Fleer Michael Jordan rookie card PSA 9" "your-api-key"
```

### Example Output

```
[12:34:56 INF] Starting Card Listing Agent...
[12:34:56 INF] Raw Description: 2023 Topps Ronald Acuna Jr rookie card, mint condition
[12:34:58 INF] ✅ Successfully processed card listing!
[12:34:58 INF] Title: 2023 Topps Ronald Acuna Jr. Rookie Card #1 - Mint Condition
[12:34:58 INF] Description: Beautiful rookie card featuring Ronald Acuna Jr. in pristine mint condition. Sharp corners, perfect centering, and vibrant colors make this an excellent addition to any collection. The 2023 Topps base set is highly regarded for its classic design and this particular card showcases Acuna's breakout season perfectly.
[12:34:58 INF] Suggested Price: $15.75
[12:34:58 INF] Category: Baseball
[12:34:58 INF] Tags: Ronald Acuna Jr, Rookie Card, Baseball, 2023 Topps, Mint Condition, Atlanta Braves
```

## Features

- **Advanced Error Handling**: Comprehensive logging and graceful failure handling
- **Input Validation**: Validates both API responses and generated card listings
- **Flexible API Key Configuration**: Supports environment variables and command-line arguments
- **JSON Response cleaning**: Automatically handles markdown formatting in API responses
- **Rich Logging**: Detailed logging for debugging and monitoring

## Integration with Sports Card Store

The generated `CardListing` objects can be used to:

1. **Pre-populate inventory forms** with AI-generated titles and descriptions
2. **Research pricing** for new card acquisitions
3. **Generate listing content** for marketplace platforms
4. **Standardize descriptions** across inventory management

The `CardListing` model is available in the `SportsCardStore.Shared` project for use across the entire application.

## Error Handling

The agent handles various error conditions:

- **Invalid API keys**: Clear error messages for authentication failures
- **Network issues**: Graceful handling of connection problems
- **Malformed responses**: JSON parsing errors and validation failures
- **Rate limiting**: Respects API rate limits and provides appropriate logging

## Development Notes

- Built using Clean Architecture patterns
- Follows the same project structure as other Sports Card Store agents
- Uses `System.Text.Json` for all JSON operations
- Implements dependency injection for testability
- Comprehensive logging via `Microsoft.Extensions.Logging`

## Future Enhancements

- Batch processing for multiple card descriptions
- Integration with image analysis APIs
- Caching for repeated descriptions
- Custom prompt templates for different card types
- Integration with real-time pricing APIs
