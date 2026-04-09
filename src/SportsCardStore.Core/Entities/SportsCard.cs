using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SportsCardStore.Core.Enums;

namespace SportsCardStore.Core.Entities;

/// <summary>
/// Represents a sports trading card in the inventory
/// </summary>
public class SportsCard
{
    /// <summary>
    /// Unique identifier for the sports card
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Name of the player featured on the card
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string PlayerName { get; set; } = string.Empty;

    /// <summary>
    /// Year the card was produced
    /// </summary>
    [Required]
    [Range(1800, 2100, ErrorMessage = "Year must be between 1800 and 2100")]
    public int Year { get; set; }

    /// <summary>
    /// Brand/manufacturer of the card (e.g., Topps, Panini, Upper Deck)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    /// Set name (e.g., "Topps Chrome", "Bowman's Best", "Heritage")
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string SetName { get; set; } = string.Empty;

    /// <summary>
    /// Card number within the set
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string CardNumber { get; set; } = string.Empty;

    /// <summary>
    /// Sport category (Baseball, Football, Basketball, Hockey)
    /// </summary>
    [Required]
    public Category Sport { get; set; }

    /// <summary>
    /// Team the player was on when the card was made
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Team { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if this is a rookie card
    /// </summary>
    [Required]
    public bool IsRookie { get; set; }

    /// <summary>
    /// Indicates if this card has an autograph
    /// </summary>
    [Required]
    public bool IsAutograph { get; set; }

    /// <summary>
    /// Indicates if this card has a relic/memorabilia piece
    /// </summary>
    [Required]
    public bool IsRelic { get; set; }

    /// <summary>
    /// Grade assigned by the grading company (0-10 scale, with decimals)
    /// </summary>
    [Range(0.0, 10.0, ErrorMessage = "Grade must be between 0.0 and 10.0")]
    [Column(TypeName = "decimal(3,1)")]
    public decimal? Grade { get; set; }

    /// <summary>
    /// Company that graded the card (PSA, BGS, SGC, or Raw for ungraded)
    /// </summary>
    [Required]
    public GradingCompany GradingCompany { get; set; }

    /// <summary>
    /// Additional condition notes or description
    /// </summary>
    [MaxLength(200)]
    public string? Condition { get; set; }

    /// <summary>
    /// Current market price of the card
    /// </summary>
    [Required]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between $0.01 and $999,999.99")]
    [Column(TypeName = "decimal(8,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// Number of cards available in inventory
    /// </summary>
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
    public int Quantity { get; set; }

    /// <summary>
    /// URL to the card image
    /// </summary>
    [MaxLength(500)]
    [Url(ErrorMessage = "Invalid URL format")]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Detailed description of the card
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Whether the card is available for purchase
    /// </summary>
    [Required]
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Date and time when the card was added to inventory
    /// </summary>
    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the card information was last updated
    /// </summary>
    [Required]
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
}