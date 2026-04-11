using SportsCardStore.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace SportsCardStore.Shared.Models
{
    /// <summary>
    /// Request object for card pricing research
    /// </summary>
    public class CardPricingRequest
    {
        [Required]
        [MaxLength(100)]
        public string PlayerName { get; set; } = string.Empty;
        
        [Required]
        [Range(1800, 2100)]
        public int Year { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Brand { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string SetName { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? ParallelName { get; set; }
        
        [Range(1, 10000)]
        public int? PrintRun { get; set; }
        
        [Required]
        public GradingCompany GradingCompany { get; set; }
        
        [Range(0.1, 10.0)]
        public decimal? Grade { get; set; }
    }

    /// <summary>
    /// Result from a pricing source containing price data and sales information
    /// </summary>
    public class PricingResult
    {
        public string SourceName { get; set; } = string.Empty;
        public decimal? LowPrice { get; set; }
        public decimal? HighPrice { get; set; }
        public decimal? AveragePrice { get; set; }
        public int SalesCount { get; set; }
        public string? SourceUrl { get; set; }
        public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;
        public List<RecentSale> RecentSales { get; set; } = new();
    }

    /// <summary>
    /// Individual recent sale data point
    /// </summary>
    public class RecentSale
    {
        public decimal Price { get; set; }
        public DateTime SaleDate { get; set; }
        public string? Platform { get; set; }
        public string? ListingTitle { get; set; }
    }

    /// <summary>
    /// Complete price research response with suggested pricing and confidence data
    /// </summary>
    public class PriceResearchResponse
    {
        public string PlayerName { get; set; } = string.Empty;
        public string CardDescription { get; set; } = string.Empty;
        public decimal? SuggestedLowPrice { get; set; }
        public decimal? SuggestedHighPrice { get; set; }
        public decimal? SuggestedListingPrice { get; set; }  // midpoint + small margin
        public string DataSource { get; set; } = string.Empty;
        public int SalesAnalyzed { get; set; }
        public List<RecentSale> RecentSales { get; set; } = new();
        public string? SourceUrl { get; set; }
        public string Confidence { get; set; } = string.Empty;  // "High" / "Medium" / "Low"
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}