using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PriceResearchAgent.Interfaces;
using SportsCardStore.Shared.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace PriceResearchAgent.Sources;

/// <summary>
/// Pricing source that queries eBay's Finding API for completed/sold listing data
/// </summary>
public class EbayPricingSource : IPricingSource
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EbayPricingSource> _logger;
    private readonly IConfiguration _configuration;
    private const string EbayApiBaseUrl = "https://svcs.ebay.com/services/search/FindingService/v1";

    public string SourceName => "eBay Finding API";

    public EbayPricingSource(HttpClient httpClient, ILogger<EbayPricingSource> logger, IConfiguration configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<PricingResult?> GetPricingAsync(CardPricingRequest request)
    {
        try
        {
            _logger.LogInformation("Searching eBay for completed sales data: {Player} {Year} {Brand} {Set}", 
                request.PlayerName, request.Year, request.Brand, request.SetName);

            var appId = GetAppId();
            if (string.IsNullOrWhiteSpace(appId))
            {
                _logger.LogError("eBay App ID not configured");
                return null;
            }

            var searchQuery = BuildSearchQuery(request);
            var searchUrl = BuildSearchUrl(searchQuery, appId);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "SportsCardStore-PriceResearchAgent/1.0");

            var response = await _httpClient.GetAsync(searchUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("eBay API error {StatusCode}: {Error}", response.StatusCode, errorContent);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var searchResponse = JsonSerializer.Deserialize<EbayFindingResponse>(content, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            var items = searchResponse?.FindCompletedItemsResponse?.FirstOrDefault()?.SearchResult?.FirstOrDefault()?.Item;
            if (items == null || !items.Any())
            {
                _logger.LogInformation("No eBay completed sales found for search query: {Query}", searchQuery);
                return new PricingResult
                {
                    SourceName = SourceName,
                    SalesCount = 0
                };
            }

            return ProcessSearchResults(items, request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error querying eBay API");
            return new PricingResult
            {
                SourceName = SourceName,
                SalesCount = 0
            };
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error parsing eBay API response");
            return new PricingResult
            {
                SourceName = SourceName,
                SalesCount = 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error querying eBay pricing");
            return new PricingResult
            {
                SourceName = SourceName,
                SalesCount = 0
            };
        }
    }

    private string? GetAppId()
    {
        // Try user secrets first, then configuration
        return _configuration["eBay:AppId"] ?? 
               Environment.GetEnvironmentVariable("EBAY_APP_ID");
    }

    private string BuildSearchQuery(CardPricingRequest request)
    {
        var parts = new List<string>
        {
            request.PlayerName,
            request.Year.ToString(),
            request.Brand,
            request.SetName
        };

        if (!string.IsNullOrWhiteSpace(request.ParallelName))
        {
            parts.Add(request.ParallelName);
        }

        if (request.Grade.HasValue && request.GradingCompany != SportsCardStore.Core.Enums.GradingCompany.Raw)
        {
            parts.Add($"{request.GradingCompany} {request.Grade}");
        }

        return string.Join(" ", parts);
    }

    private string BuildSearchUrl(string searchQuery, string appId)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["OPERATION-NAME"] = "findCompletedItems",
            ["SERVICE-VERSION"] = "1.0.0", 
            ["SECURITY-APPNAME"] = appId,
            ["RESPONSE-DATA-FORMAT"] = "JSON",
            ["REST-PAYLOAD"] = "",
            ["keywords"] = searchQuery,
            ["categoryId"] = "261328", // Sports Trading Cards category
            ["itemFilter(0).name"] = "SoldItemsOnly",
            ["itemFilter(0).value"] = "true",
            ["itemFilter(1).name"] = "ListingType",
            ["itemFilter(1).value(0)"] = "Auction",
            ["itemFilter(1).value(1)"] = "FixedPrice",
            ["itemFilter(2).name"] = "Country",
            ["itemFilter(2).value"] = "US",
            ["sortOrder"] = "EndTimeSoonest",
            ["paginationInput.entriesPerPage"] = "50"
        };

        var queryString = string.Join("&", queryParams.Select(kvp => 
            $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));

        return $"{EbayApiBaseUrl}?{queryString}";
    }

    private PricingResult ProcessSearchResults(List<EbayCompletedItem> items, CardPricingRequest request)
    {
        var validItems = items
            .Where(item => IsValidMatch(item, request))
            .Where(item => item.SellingStatus?.ConvertedCurrentPrice?.Value != null && item.SellingStatus.ConvertedCurrentPrice.Value > 0)
            .ToList();

        if (!validItems.Any())
        {
            _logger.LogInformation("No valid completed sales found in eBay search results");
            return new PricingResult
            {
                SourceName = SourceName,
                SalesCount = 0
            };
        }

        var prices = validItems.Select(item => item.SellingStatus!.ConvertedCurrentPrice!.Value).ToList();
        var recentSales = validItems.Take(10).Select(item => new RecentSale
        {
            Price = item.SellingStatus!.ConvertedCurrentPrice!.Value,
            SaleDate = DateTime.TryParse(item.ListingInfo?.EndTime, out var endTime) ? endTime : DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)),
            Platform = "eBay",
            ListingTitle = item.Title ?? "Unknown"
        }).ToList();

        var result = new PricingResult
        {
            SourceName = SourceName,
            LowPrice = prices.Min(),
            HighPrice = prices.Max(),
            AveragePrice = prices.Average(),
            SalesCount = prices.Count,
            SourceUrl = $"https://www.ebay.com/sch/i.html?_nkw={HttpUtility.UrlEncode(BuildSearchQuery(request))}&LH_Sold=1",
            RecentSales = recentSales
        };

        _logger.LogInformation("eBay completed sales found: Low=${Low:F2}, High=${High:F2}, Average=${Avg:F2}, Sales={Count}", 
            result.LowPrice, result.HighPrice, result.AveragePrice, result.SalesCount);

        return result;
    }

    private bool IsValidMatch(EbayCompletedItem item, CardPricingRequest request)
    {
        if (string.IsNullOrWhiteSpace(item.Title))
            return false;

        var title = item.Title.ToLowerInvariant();
        var playerName = request.PlayerName.ToLowerInvariant();

        // Basic validation - must contain player name and year
        if (!title.Contains(playerName) || !title.Contains(request.Year.ToString()))
            return false;

        // Filter out obvious non-matches
        var excludeKeywords = new[] { "lot of", "collection", "box", "pack", "complete set", "break", "case" };
        if (excludeKeywords.Any(keyword => title.Contains(keyword)))
            return false;

        // Price validation - exclude extremely low or high prices that are likely errors
        var price = item.SellingStatus?.ConvertedCurrentPrice?.Value ?? 0;
        if (price < 0.50m || price > 50000m)
            return false;

        return true;
    }
}

// eBay Finding API Response Models
internal class EbayFindingResponse
{
    [JsonPropertyName("findCompletedItemsResponse")]
    public List<FindCompletedItemsResponseType>? FindCompletedItemsResponse { get; set; }
}

internal class FindCompletedItemsResponseType
{
    [JsonPropertyName("searchResult")]
    public List<SearchResultType>? SearchResult { get; set; }
}

internal class SearchResultType
{
    [JsonPropertyName("item")]
    public List<EbayCompletedItem>? Item { get; set; }
}

internal class EbayCompletedItem
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("sellingStatus")]
    public SellingStatus? SellingStatus { get; set; }

    [JsonPropertyName("listingInfo")]
    public ListingInfo? ListingInfo { get; set; }

    [JsonPropertyName("viewItemURL")]
    public string? ViewItemURL { get; set; }
}

internal class SellingStatus
{
    [JsonPropertyName("convertedCurrentPrice")]
    public ConvertedCurrentPrice? ConvertedCurrentPrice { get; set; }
}

internal class ConvertedCurrentPrice
{
    [JsonPropertyName("__value__")]
    public decimal Value { get; set; }

    [JsonPropertyName("@currencyId")]
    public string? CurrencyId { get; set; }
}

internal class ListingInfo
{
    [JsonPropertyName("endTime")]
    public string? EndTime { get; set; }
}