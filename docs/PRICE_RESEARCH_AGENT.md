# Price Research Agent — Architecture & Implementation Plan

> **Purpose:** Reference document for building the Price Research Agent (Prompt 8.3).
> Documents the dual-source architecture, interface design, and upgrade path to Card Ladder.

---

## Goal

Given a sports card description (player, year, brand, set, parallel, grade), return a suggested
price range backed by real recent sales data. The agent should be honest about its sources and
confidence level.

---

## Architecture — IPricingSource Interface

The agent is built around a `IPricingSource` interface so any data source can be swapped in
or out without rewriting the agent logic.

```csharp
public interface IPricingSource
{
    string SourceName { get; }
    Task<PricingResult?> GetPricingAsync(CardPricingRequest request);
}

public class CardPricingRequest
{
    public string PlayerName { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string SetName { get; set; } = string.Empty;
    public string? ParallelName { get; set; }
    public int? PrintRun { get; set; }
    public GradingCompany GradingCompany { get; set; }
    public decimal? Grade { get; set; }
}

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

public class RecentSale
{
    public decimal Price { get; set; }
    public DateTime SaleDate { get; set; }
    public string? Platform { get; set; }
    public string? ListingTitle { get; set; }
}
```

---

## Source Priority

```
1. Card Ladder (primary — most accurate, covers 14 platforms)
   ↓ if unavailable or no results
2. eBay Browse API (fallback — real-time completed listings)
   ↓ if still no results
3. Card Ladder web scrape (last resort — no API key needed)
```

---

## Phase 1 — Build with eBay (Current)

### Why eBay first
- Free public API (eBay Browse API)
- Well-documented with C# HttpClient examples
- Completed/sold listings provide real transaction data
- No approval process needed

### eBay Browse API approach
- Endpoint: `https://api.ebay.com/buy/browse/v1/item_summary/search`
- Filter: `buyingOptions:{AUCTION|FIXED_PRICE}` + `itemLocationCountry:US`
- Sort by: `newlyListed` to get most recent sales
- Search query: built from `{PlayerName} {Year} {Brand} {SetName} {ParallelName} {Grade}`
- Requires a free eBay Developer account and OAuth app token

### eBay implementation notes
- Strip `$` from prices before parsing
- Filter out obviously wrong results (wrong sport, wrong player)
- Look at last 30 days of completed sales for relevance
- Return low/high/average across the matched results

---

## Phase 2 — Upgrade to Card Ladder API (Future)

### Current status
Card Ladder has an enterprise API but it is not publicly documented.
It appears to require direct contact with their team.

### Action needed before building
Contact Card Ladder at [email protected] and ask:
1. Does a Pro subscription ($15/month) include API access for personal projects?
2. What does enterprise API access cost?
3. What endpoints are available — specifically sales history and CL Value by card?

### If API access is granted
- Implement `CardLadderApiPricingSource : IPricingSource`
- Authenticate with Card Ladder credentials (store in user secrets, not appsettings.json)
- Query by player + year + set + parallel — return CL Value + recent sales
- Register as the primary source in the agent's source list
- eBay drops to fallback automatically — no other code changes needed

---

## Phase 3 — Web Scrape Fallback (Option 3)

If Card Ladder API access is not available or too expensive, the agent can scrape
Card Ladder's public price guide pages directly using your existing Pro account session.

### How it works
- Use HttpClient with your Card Ladder session cookie
- Navigate to `https://www.cardladder.com/ladder?search={query}`
- Parse the CL Value and recent sales from the page HTML
- This is fragile (breaks if Card Ladder changes their HTML) but costs nothing extra

### Implementation notes
- Extract session cookie from browser after logging in to Card Ladder
- Store as a user secret, not in config
- Wrap in a try/catch — if scrape fails, fall through to eBay automatically
- Log a warning when falling back so you know the scrape broke

### Important caveat
Web scraping may violate Card Ladder's Terms of Service. Review
`https://www.cardladder.com/terms` before implementing. Given you're a paying
subscriber using it for personal/business inventory management (not redistribution),
it's likely fine — but worth checking.

---

## Agent Output

The Price Research Agent returns a `PriceResearchResponse` to the caller:

```csharp
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
```

---

## Copilot Prompt — Phase 1 (eBay)

```
Create a C# class called PriceResearchAgent in a new 
src/PriceResearchAgent/ console project in the SportsCardStore 
solution. The agent should:

1. Define an IPricingSource interface with a SourceName property 
   and GetPricingAsync(CardPricingRequest) method returning 
   Task<PricingResult?>

2. Implement EbayPricingSource : IPricingSource using the eBay 
   Browse API (https://api.ebay.com/buy/browse/v1/item_summary/search)
   - Build a search query from PlayerName, Year, Brand, SetName, 
     ParallelName, and Grade
   - Filter to US completed listings from the last 30 days
   - Return low, high, and average price across results
   - Read the eBay OAuth token from configuration

3. Implement PriceResearchAgent that:
   - Accepts a list of IPricingSource (injected)
   - Tries each source in order, stopping at the first with results
   - Returns a PriceResearchResponse with suggested low/high/listing 
     price, data source name, sales count, and confidence level
   - Accepts card details as command line arguments

4. Register sources in order: EbayPricingSource first (Card Ladder 
   will be added later as primary source)

Use ILogger for structured logging. Store eBay credentials in 
configuration (not hardcoded). Include error handling that falls 
through to the next source if one fails.
```

---

## Copilot Prompt — Phase 2 (Add Card Ladder API)

*Run this after obtaining Card Ladder API credentials*

```
Add a CardLadderApiPricingSource : IPricingSource implementation 
to the PriceResearchAgent project. It should:
- Authenticate using Card Ladder API credentials from user secrets
- Query the Card Ladder API for sales history by player/year/set/parallel
- Return the CL Value and recent sales as a PricingResult
- Read API base URL and credentials from configuration

Register CardLadderApiPricingSource FIRST in the source list in 
Program.cs so it is tried before EbayPricingSource.
```

---

## Copilot Prompt — Phase 3 (Web Scrape Fallback)

*Run this if Card Ladder API is not available*

```
Add a CardLadderWebPricingSource : IPricingSource implementation 
to the PriceResearchAgent project. It should:
- Use HttpClient with a Card Ladder session cookie from user secrets
- Navigate to Card Ladder's price guide search for the given card
- Parse the CL Value and recent sales from the HTML response
- Return null (triggering fallback to eBay) if parsing fails or 
  the session cookie is expired
- Log a warning when the scrape fails

Register CardLadderWebPricingSource FIRST in the source list, 
before EbayPricingSource. CardLadderApiPricingSource registration 
(if present) stays first.
```

---

*Last Updated: April 2026*
