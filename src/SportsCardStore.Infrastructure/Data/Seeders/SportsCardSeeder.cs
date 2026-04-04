using Microsoft.EntityFrameworkCore;
using SportsCardStore.Core.Entities;
using SportsCardStore.Core.Enums;

namespace SportsCardStore.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds the database with initial sports card data for development
/// </summary>
public static class SportsCardSeeder
{
    /// <summary>
    /// Seeds the SportsCard table with realistic sample data
    /// </summary>
    /// <param name="context">The database context</param>
    public static async Task SeedAsync(AppDbContext context)
    {
        // Check if we already have data
        if (await context.SportsCards.AnyAsync())
        {
            return; // Already seeded
        }

        var sportsCards = new List<SportsCard>
        {
            // Baseball Cards
            new SportsCard
            {
                PlayerName = "Mike Trout",
                Year = 2009,
                Brand = "Bowman Chrome",
                CardNumber = "1",
                Sport = Category.Baseball,
                Team = "Los Angeles Angels",
                Grade = 9.5m,
                GradingCompany = GradingCompany.BGS,
                Condition = "Near Mint+",
                Price = 450.00m,
                Quantity = 1,
                ImageUrl = "https://example.com/trout-2009-bowman.jpg",
                Description = "Mike Trout rookie card from 2009 Bowman Chrome. BGS 9.5 graded with excellent centering.",
                IsAvailable = true
            },
            new SportsCard
            {
                PlayerName = "Ronald Acuna Jr.",
                Year = 2018,
                Brand = "Topps Chrome",
                CardNumber = "193",
                Sport = Category.Baseball,
                Team = "Atlanta Braves",
                Grade = 10.0m,
                GradingCompany = GradingCompany.PSA,
                Condition = "Gem Mint",
                Price = 275.00m,
                Quantity = 1,
                ImageUrl = "https://example.com/acuna-2018-chrome.jpg",
                Description = "Ronald Acuna Jr. rookie card. PSA 10 perfect grade with sharp corners and centering.",
                IsAvailable = true
            },
            new SportsCard
            {
                PlayerName = "Derek Jeter",
                Year = 1993,
                Brand = "Upper Deck",
                CardNumber = "449",
                Sport = Category.Baseball,
                Team = "New York Yankees", 
                Grade = null,
                GradingCompany = GradingCompany.Raw,
                Condition = "Near Mint - slight corner wear",
                Price = 125.00m,
                Quantity = 2,
                ImageUrl = "https://example.com/jeter-1993-ud.jpg",
                Description = "Derek Jeter rookie card from 1993 Upper Deck. Raw ungraded card in excellent condition.",
                IsAvailable = true
            },
            // Football Cards
            new SportsCard
            {
                PlayerName = "Tom Brady",
                Year = 2000,
                Brand = "Playoff Contenders",
                CardNumber = "144",
                Sport = Category.Football,
                Team = "New England Patriots",
                Grade = 8.5m,
                GradingCompany = GradingCompany.PSA,
                Condition = "Near Mint+",
                Price = 385.00m,
                Quantity = 1,
                ImageUrl = "https://example.com/brady-2000-contenders.jpg",
                Description = "Tom Brady rookie ticket autograph card. PSA 8.5 with clean signature.",
                IsAvailable = true
            },
            new SportsCard
            {
                PlayerName = "Justin Herbert",
                Year = 2020,
                Brand = "Panini Prizm",
                CardNumber = "325",
                Sport = Category.Football,
                Team = "Los Angeles Chargers",
                Grade = 9.0m,
                GradingCompany = GradingCompany.BGS,
                Condition = "Mint",
                Price = 95.00m,
                Quantity = 3,
                ImageUrl = "https://example.com/herbert-2020-prizm.jpg",
                Description = "Justin Herbert rookie card from 2020 Panini Prizm. BGS 9 with great eye appeal.",
                IsAvailable = true
            },
            new SportsCard
            {
                PlayerName = "Joe Burrow",
                Year = 2020,
                Brand = "Panini Select",
                CardNumber = "46",
                Sport = Category.Football,
                Team = "Cincinnati Bengals",
                Grade = null,
                GradingCompany = GradingCompany.Raw,
                Condition = "Near Mint",
                Price = 45.00m,
                Quantity = 5,
                ImageUrl = "https://example.com/burrow-2020-select.jpg",
                Description = "Joe Burrow rookie card. Raw card with sharp corners and good centering.",
                IsAvailable = true
            },
            // Basketball Cards
            new SportsCard
            {
                PlayerName = "LeBron James",
                Year = 2003,
                Brand = "Upper Deck Exquisite",
                CardNumber = "78",
                Sport = Category.Basketball,
                Team = "Cleveland Cavaliers",
                Grade = 8.0m,
                GradingCompany = GradingCompany.PSA,
                Condition = "Near Mint",
                Price = 495.00m,
                Quantity = 1,
                ImageUrl = "https://example.com/lebron-2003-exquisite.jpg",
                Description = "LeBron James rookie patch autograph. PSA 8 with beautiful patch piece.",
                IsAvailable = true
            },
            new SportsCard
            {
                PlayerName = "Luka Doncic",
                Year = 2018,
                Brand = "Panini Prizm",
                CardNumber = "280",
                Sport = Category.Basketball,
                Team = "Dallas Mavericks",
                Grade = 10.0m,
                GradingCompany = GradingCompany.BGS,
                Condition = "Pristine",
                Price = 225.00m,
                Quantity = 1,
                ImageUrl = "https://example.com/doncic-2018-prizm.jpg",
                Description = "Luka Doncic rookie card. BGS pristine 10 with perfect sub-grades.",
                IsAvailable = true
            },
            new SportsCard
            {
                PlayerName = "Michael Jordan",
                Year = 1986,
                Brand = "Fleer",
                CardNumber = "57",
                Sport = Category.Basketball,
                Team = "Chicago Bulls",
                Grade = null,
                GradingCompany = GradingCompany.Raw,
                Condition = "Very Good - some edge wear",
                Price = 175.00m,
                Quantity = 1,
                ImageUrl = "https://example.com/jordan-1986-fleer.jpg",
                Description = "Michael Jordan rookie card from 1986 Fleer. Raw card with typical edge wear for the era.",
                IsAvailable = true
            },
            new SportsCard
            {
                PlayerName = "Zion Williamson",
                Year = 2019,
                Brand = "Panini Contenders",
                CardNumber = "1",
                Sport = Category.Basketball,
                Team = "New Orleans Pelicans",
                Grade = 9.0m,
                GradingCompany = GradingCompany.SGC,
                Condition = "Mint",
                Price = 85.00m,
                Quantity = 2,
                ImageUrl = "https://example.com/zion-2019-contenders.jpg",
                Description = "Zion Williamson rookie ticket. SGC 9 with clean autograph and nice centering.",
                IsAvailable = true
            }
        };

        await context.SportsCards.AddRangeAsync(sportsCards);
        await context.SaveChangesAsync();
    }
}