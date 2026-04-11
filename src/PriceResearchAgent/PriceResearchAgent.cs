using Microsoft.Extensions.Logging;
using PriceResearchAgent.Interfaces;
using SportsCardStore.Shared.Models;

namespace PriceResearchAgent;

/// <summary>
/// Main agent that coordinates pricing research across multiple data sources
/// </summary>
public class PriceResearchAgent
{
    private readonly IEnumerable<IPricingSource> _pricingSources;
    private readonly ILogger<PriceResearchAgent> _logger;

    public PriceResearchAgent(IEnumerable<IPricingSource> pricingSources, ILogger<PriceResearchAgent> logger)
    {
        _pricingSources = pricingSources ?? throw new ArgumentNullException(nameof(pricingSources));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Researches pricing for a card by trying each source in priority order until results are found
    /// </summary>
    /// <param name="request">Card details to research</param>
    /// <returns>Price research response with suggested pricing and confidence data</returns>
    public async Task<PriceResearchResponse?> ResearchPricingAsync(CardPricingRequest request)
    {
        if (request == null)
        {
            _logger.LogWarning("Price research request is null");
            return null;
        }

        _logger.LogInformation("Starting price research for: {Player} {Year} {Brand} {Set}", 
            request.PlayerName, request.Year, request.Brand, request.SetName);

        var cardDescription = BuildCardDescription(request);

        foreach (var source in _pricingSources)
        {
            try
            {
                _logger.LogInformation("Trying pricing source: {SourceName}", source.SourceName);
                
                var result = await source.GetPricingAsync(request);
                
                if (result != null && result.SalesCount > 0)
                {
                    _logger.LogInformation("Pricing data found from {SourceName}: {Sales} sales", 
                        source.SourceName, result.SalesCount);
                    
                    return BuildPriceResearchResponse(request, cardDescription, result);
                }
                else
                {
                    _logger.LogInformation("No pricing data from {SourceName}, trying next source", source.SourceName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pricing from {SourceName}, trying next source", source.SourceName);
                continue;
            }
        }

        _logger.LogWarning("No pricing data found from any source for card: {Description}", cardDescription);
        
        return new PriceResearchResponse
        {
            PlayerName = request.PlayerName,
            CardDescription = cardDescription,
            DataSource = "None",
            Confidence = "No Data",
            SalesAnalyzed = 0
        };
    }

    private string BuildCardDescription(CardPricingRequest request)
    {
        var parts = new List<string>
        {
            $"{request.Year} {request.Brand} {request.SetName}",
            request.PlayerName
        };

        if (!string.IsNullOrWhiteSpace(request.ParallelName))
        {
            parts.Add($"({request.ParallelName})");
        }

        if (request.Grade.HasValue && request.GradingCompany != SportsCardStore.Core.Enums.GradingCompany.Raw)
        {
            parts.Add($"{request.GradingCompany} {request.Grade}");
        }
        else if (request.GradingCompany == SportsCardStore.Core.Enums.GradingCompany.Raw)
        {
            parts.Add("Raw/Ungraded");
        }

        return string.Join(" ", parts);
    }

    private PriceResearchResponse BuildPriceResearchResponse(CardPricingRequest request, string cardDescription, PricingResult result)
    {
        var confidence = DetermineConfidence(result.SalesCount);
        var (suggestedLow, suggestedHigh, suggestedListing) = CalculateSuggestedPrices(result);

        var response = new PriceResearchResponse
        {
            PlayerName = request.PlayerName,
            CardDescription = cardDescription,
            SuggestedLowPrice = suggestedLow,
            SuggestedHighPrice = suggestedHigh,
            SuggestedListingPrice = suggestedListing,
            DataSource = result.SourceName,
            SalesAnalyzed = result.SalesCount,
            RecentSales = result.RecentSales,
            SourceUrl = result.SourceUrl,
            Confidence = confidence
        };

        _logger.LogInformation("Price research complete: Low=${Low:F2}, High=${High:F2}, Listing=${List:F2}, Confidence={Confidence}", 
            response.SuggestedLowPrice, response.SuggestedHighPrice, response.SuggestedListingPrice, response.Confidence);

        return response;
    }

    private string DetermineConfidence(int salesCount)
    {
        return salesCount switch
        {
            >= 10 => "High",
            >= 5 => "Medium",
            >= 1 => "Low",
            _ => "No Data"
        };
    }

    private (decimal? low, decimal? high, decimal? listing) CalculateSuggestedPrices(PricingResult result)
    {
        if (!result.LowPrice.HasValue || !result.HighPrice.HasValue || !result.AveragePrice.HasValue)
        {
            return (null, null, null);
        }

        var low = result.LowPrice.Value;
        var high = result.HighPrice.Value;
        var average = result.AveragePrice.Value;

        // Suggested listing price: start slightly above average for negotiation room
        var listingMultiplier = result.SalesCount >= 10 ? 1.05m : 1.10m; // Less markup if lots of data
        var suggestedListing = Math.Round(average * listingMultiplier, 2);

        // Ensure listing price is within reasonable bounds
        suggestedListing = Math.Max(suggestedListing, low * 1.02m); // At least 2% above low
        suggestedListing = Math.Min(suggestedListing, high * 0.95m); // No more than 95% of high

        return (
            Math.Round(low, 2),
            Math.Round(high, 2), 
            Math.Round(suggestedListing, 2)
        );
    }
}