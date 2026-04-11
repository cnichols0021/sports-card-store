using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PriceResearchAgent;
using PriceResearchAgent.Interfaces;
using PriceResearchAgent.Sources;
using SportsCardStore.Core.Enums;
using SportsCardStore.Shared.Models;

namespace PriceResearchAgent;

public class Program
{
    private static ILogger<Program>? _logger;

    public static async Task<int> Main(string[] args)
    {
        // Configure services
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        // Add configuration
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        // Add HttpClient for API calls
        services.AddHttpClient<EbayPricingSource>();

        // Register pricing sources in priority order
        services.AddTransient<IPricingSource, EbayPricingSource>();

        // Register the main agent
        services.AddTransient<PriceResearchAgent>();

        var serviceProvider = services.BuildServiceProvider();
        _logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            // Parse command line arguments
            var request = ParseArguments(args);
            if (request == null)
            {
                ShowUsage();
                return 1;
            }

            _logger.LogInformation("Starting Price Research Agent...");
            LogRequestDetails(request);

            // Run price research
            var agent = serviceProvider.GetRequiredService<PriceResearchAgent>();
            var result = await agent.ResearchPricingAsync(request);

            if (result != null)
            {
                DisplayResults(result);
                return 0; // Success
            }
            else
            {
                _logger.LogError("❌ Failed to research card pricing");
                return 1; // Failure
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unhandled exception occurred");
            return 1;
        }
        finally
        {
            serviceProvider.Dispose();
        }
    }

    private static CardPricingRequest? ParseArguments(string[] args)
    {
        if (args.Length < 4)
        {
            return null;
        }

        try
        {
            var request = new CardPricingRequest
            {
                PlayerName = args[0],
                Year = int.Parse(args[1]),
                Brand = args[2],
                SetName = args[3]
            };

            // Optional parameters
            if (args.Length > 4 && !string.IsNullOrWhiteSpace(args[4]))
            {
                request.ParallelName = args[4];
            }

            if (args.Length > 5 && Enum.TryParse<GradingCompany>(args[5], true, out var gradingCompany))
            {
                request.GradingCompany = gradingCompany;
            }
            else
            {
                request.GradingCompany = GradingCompany.Raw;
            }

            if (args.Length > 6 && decimal.TryParse(args[6], out var grade))
            {
                request.Grade = grade;
            }

            return request;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error parsing command line arguments");
            return null;
        }
    }

    private static void ShowUsage()
    {
        _logger?.LogError("Usage: PriceResearchAgent.exe <player> <year> <brand> <set> [parallel] [grading_company] [grade]");
        _logger?.LogError("");
        _logger?.LogError("Examples:");
        _logger?.LogError("  PriceResearchAgent.exe \"Ronald Acuna Jr\" 2018 \"Topps\" \"Chrome\" \"\" \"PSA\" 10");
        _logger?.LogError("  PriceResearchAgent.exe \"Mike Trout\" 2011 \"Topps\" \"Update\" \"Gold\" \"BGS\" 9.5");
        _logger?.LogError("  PriceResearchAgent.exe \"Connor Bedard\" 2023 \"Upper Deck\" \"Series 1\" \"\" \"Raw\" \"\"");
        _logger?.LogError("");
        _logger?.LogError("Grading Companies: Raw, PSA, BGS, SGC, CGS");
        _logger?.LogError("Grade: 0.1 to 10.0 (leave empty for raw cards)");
        _logger?.LogError("");
        _logger?.LogError("Configuration:");
        _logger?.LogError("  Set eBay access token via user secrets or EBAY_ACCESS_TOKEN environment variable");
        _logger?.LogError("  Example: dotnet user-secrets set \"eBay:AccessToken\" \"your-token-here\"");
    }

    private static void LogRequestDetails(CardPricingRequest request)
    {
        _logger?.LogInformation("Request Details:");
        _logger?.LogInformation("  Player: {Player}", request.PlayerName);
        _logger?.LogInformation("  Year: {Year}", request.Year);
        _logger?.LogInformation("  Brand: {Brand}", request.Brand);
        _logger?.LogInformation("  Set: {Set}", request.SetName);
        
        if (!string.IsNullOrWhiteSpace(request.ParallelName))
        {
            _logger?.LogInformation("  Parallel: {Parallel}", request.ParallelName);
        }
        
        _logger?.LogInformation("  Grading Company: {GradingCompany}", request.GradingCompany);
        
        if (request.Grade.HasValue)
        {
            _logger?.LogInformation("  Grade: {Grade}", request.Grade);
        }
    }

    private static void DisplayResults(PriceResearchResponse result)
    {
        _logger?.LogInformation("");
        _logger?.LogInformation("🎯 Price Research Results");
        _logger?.LogInformation("═══════════════════════════════════════════════════");
        _logger?.LogInformation("Card: {Description}", result.CardDescription);
        _logger?.LogInformation("Data Source: {Source}", result.DataSource);
        _logger?.LogInformation("Sales Analyzed: {Count}", result.SalesAnalyzed);
        _logger?.LogInformation("Confidence: {Confidence}", result.Confidence);
        _logger?.LogInformation("");

        if (result.SuggestedLowPrice.HasValue && result.SuggestedHighPrice.HasValue)
        {
            _logger?.LogInformation("💰 Suggested Pricing:");
            _logger?.LogInformation("  Low Price:     ${Low:F2}", result.SuggestedLowPrice);
            _logger?.LogInformation("  High Price:    ${High:F2}", result.SuggestedHighPrice);
            
            if (result.SuggestedListingPrice.HasValue)
            {
                _logger?.LogInformation("  Listing Price: ${List:F2} (recommended starting point)", result.SuggestedListingPrice);
            }
        }
        else
        {
            _logger?.LogInformation("💰 No pricing data available");
        }

        if (result.RecentSales.Any())
        {
            _logger?.LogInformation("");
            _logger?.LogInformation("📊 Recent Sales (up to 10):");
            foreach (var sale in result.RecentSales.Take(10))
            {
                _logger?.LogInformation("  ${Price:F2} - {Platform} - {Title}", 
                    sale.Price, sale.Platform ?? "Unknown", 
                    (sale.ListingTitle?.Length > 50 ? sale.ListingTitle.Substring(0, 47) + "..." : sale.ListingTitle) ?? "No title");
            }
        }

        if (!string.IsNullOrWhiteSpace(result.SourceUrl))
        {
            _logger?.LogInformation("");
            _logger?.LogInformation("🔗 Source URL: {Url}", result.SourceUrl);
        }

        _logger?.LogInformation("");
        _logger?.LogInformation("Generated: {Time:yyyy-MM-dd HH:mm:ss} UTC", result.GeneratedAt);
    }
}