using SportsCardStore.Shared.Models;

namespace PriceResearchAgent.Interfaces;

/// <summary>
/// Interface for pricing data sources that can be queried for card pricing information
/// </summary>
public interface IPricingSource
{
    /// <summary>
    /// Human-readable source name for logging and display purposes
    /// </summary>
    string SourceName { get; }

    /// <summary>
    /// Retrieves pricing data for the specified card from this source
    /// </summary>
    /// <param name="request">Card details to search for</param>
    /// <returns>Pricing result if found, null if no data available</returns>
    Task<PricingResult?> GetPricingAsync(CardPricingRequest request);
}