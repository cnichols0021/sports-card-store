using SportsCardStore.Core.Entities;
using SportsCardStore.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace SportsCardStore.Shared.Models
{
    public class CreateSportsCardRequest
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

        [Required]
        [MaxLength(20)]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        public Category Sport { get; set; }

        [Required]
        [MaxLength(50)]
        public string Team { get; set; } = string.Empty;

        [Required]
        public bool IsRookie { get; set; }

        [Required]
        public bool IsAutograph { get; set; }

        [Required]
        public bool IsRelic { get; set; }

        [Range(0.1, 10.0)]
        public decimal? Grade { get; set; }

        [Required]
        public GradingCompany GradingCompany { get; set; }

        [MaxLength(200)]
        public string? Condition { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Url]
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsAvailable { get; set; } = true;
    }

    public class UpdateSportsCardRequest
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

        [Required]
        [MaxLength(20)]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        public Category Sport { get; set; }

        [Required]
        [MaxLength(50)]
        public string Team { get; set; } = string.Empty;

        [Required]
        public bool IsRookie { get; set; }

        [Required]
        public bool IsAutograph { get; set; }

        [Required]
        public bool IsRelic { get; set; }

        [Range(0.1, 10.0)]
        public decimal? Grade { get; set; }

        [Required]
        public GradingCompany GradingCompany { get; set; }

        [MaxLength(200)]
        public string? Condition { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Url]
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsAvailable { get; set; } = true;
    }

    public class SportsCardResponse
    {
        public int Id { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string SetName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public Category Sport { get; set; }
        public string Team { get; set; } = string.Empty;
        public bool IsRookie { get; set; }
        public bool IsAutograph { get; set; }
        public bool IsRelic { get; set; }
        public decimal? Grade { get; set; }
        public GradingCompany GradingCompany { get; set; }
        public string? Condition { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }    
        public DateTime UpdatedDate { get; set; }
    }

    public class PagedSportsCardResponse
    {
        public IEnumerable<SportsCardResponse> Items { get; set; } = new List<SportsCardResponse>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}