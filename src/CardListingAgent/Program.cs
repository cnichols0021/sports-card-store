using CardListingAgent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CardListingAgent;

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

        // Add HttpClient for API calls
        services.AddHttpClient<CardListingAgent>();

        var serviceProvider = services.BuildServiceProvider();
        _logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            // Validate command line arguments
            if (args.Length == 0)
            {
                _logger.LogError("Usage: CardListingAgent.exe \"<raw card description>\" [api-key]");
                _logger.LogError("Example: CardListingAgent.exe \"2023 Topps Ronald Acuna Jr rookie card, mint condition\" \"your-api-key\"");
                _logger.LogError("Note: API key can also be provided via ANTHROPIC_API_KEY environment variable");
                return 1;
            }

            var rawDescription = args[0];
            var apiKey = GetApiKey(args);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogError("API key must be provided either as second argument or via ANTHROPIC_API_KEY environment variable");
                return 1;
            }

            _logger.LogInformation("Starting Card Listing Agent...");
            _logger.LogInformation("Raw Description: {Description}", rawDescription);

            // Create and configure the agent
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            var agentLogger = serviceProvider.GetRequiredService<ILogger<CardListingAgent>>();
            var agent = new CardListingAgent(httpClient, agentLogger, apiKey);

            // Process the card description
            var result = await agent.ProcessCardDescriptionAsync(rawDescription);

            if (result != null)
            {
                _logger.LogInformation("✅ Successfully processed card listing!");
                _logger.LogInformation("Title: {Title}", result.Title);
                _logger.LogInformation("Description: {Description}", result.Description);
                _logger.LogInformation("Suggested Price: ${Price:F2}", result.SuggestedPrice);
                _logger.LogInformation("Category: {Category}", result.Category);
                _logger.LogInformation("Tags: {Tags}", string.Join(", ", result.Tags));
                
                return 0; // Success
            }
            else
            {
                _logger.LogError("❌ Failed to process card description");
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

    private static string? GetApiKey(string[] args)
    {
        // Check command line argument first
        if (args.Length >= 2 && !string.IsNullOrWhiteSpace(args[1]))
        {
            return args[1];
        }

        // Fall back to environment variable
        return Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY");
    }
}