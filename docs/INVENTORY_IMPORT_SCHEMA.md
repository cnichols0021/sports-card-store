# Inventory Import Agent — Excel Column Schema

> **Purpose:** Reference document for building the Inventory Import Agent (Phase 8.2).
> Maps the Excel card tracker columns directly to the `SportsCard` entity fields in the Core project.
> Use this when writing the Copilot prompt for the agent.

---

## Column Mapping

| Excel Column | SportsCard Entity Field | Type | Notes |
|---|---|---|---|
| Player Name | `PlayerName` | string | Required. Max 100 chars. |
| Team | `Team` | string | Required. Max 50 chars. |
| Brand | `Brand` | string | Required. Max 50 chars. e.g. Topps, Panini, Upper Deck |
| Set Name | `SetName` | string | Required. e.g. "Topps Chrome", "Bowman's Best", "Heritage" |
| Card Number | `CardNumber` | string | Required. Max 20 chars. e.g. "27", "RC-14" |
| Year | `Year` | int | Required. Range 1800–2100. |
| Sport | `Sport` | enum (Category) | Required. Baseball / Football / Basketball / Hockey |
| Rookie | `IsRookie` | bool | true/false or yes/no in Excel |
| Autograph | `IsAutograph` | bool | true/false or yes/no in Excel |
| Relic | `IsRelic` | bool | true/false or yes/no in Excel |
| Grading Company | `GradingCompany` | enum | Raw / PSA / BGS / SGC. Use "Raw" for ungraded cards. |
| Card Grade | `Grade` | decimal? | Nullable. 0.0–10.0 scale. Leave blank for Raw cards. |
| Condition | `Condition` | string | Optional. Max 200 chars. e.g. "Near Mint", "Gem Mint" |
| Price | `Price` | decimal | Required. e.g. 49.99 |
| Quantity | `Quantity` | int | Required. Number of copies available. |
| Description | `Description` | string | Optional. Max 1000 chars. |
| Image Url | `ImageUrl` | string | Optional. Populated after scanning + uploading to Blob Storage. |
| Is Available | `IsAvailable` | bool | true = listed for sale. Default true on import. |

---

## Columns Removed From Original List

| Removed Column | Reason |
|---|---|
| Graded (boolean) | Redundant — GradingCompany = "Raw" means ungraded. No need for a separate flag. |
| Title | Lives on `CardListing`, not `SportsCard`. Not part of inventory tracking. |

---

## Columns Added vs. Original List

| Added Column | Reason |
|---|---|
| Set Name | Critical for identifying cards — Brand alone isn't enough (Topps has dozens of sets per year) |
| Card Number | Required to uniquely identify a card within a set |
| Image Url | Needed for Blob Storage pipeline — maps to scanned card images |
| Is Available | Controls whether card shows as listed for sale — default true on import |

---

## Enum Reference

### Sport / Category
```
Baseball
Football
Basketball
Hockey
```

### Grading Company
```
Raw      // Ungraded — leave Card Grade blank
PSA      // Professional Sports Authenticator
BGS      // Beckett Grading Services
SGC      // Sportscard Guaranty Corporation
```

---

## Import Agent Notes

- **Excel file location:** `Sports_Cards_in_Order.xlsx` (approximately 477 cards)
- **Recommended library:** EPPlus or ClosedXML for Excel parsing in C#
- **Validation rules:**
  - Skip rows where PlayerName is blank
  - Raw cards must have Grade = null — do not import a grade value for Raw cards
  - Prices must be > 0
  - Year must be between 1800 and 2100
  - Default IsAvailable = true if column is blank
- **Import strategy:** Call the existing `POST /api/sportscards` endpoint rather than writing directly to the database — keeps the import agent decoupled from the data layer and exercises the real API
- **Duplicate handling:** Check if a card with the same PlayerName + Year + Brand + CardNumber already exists before inserting

---

## Copilot Prompt Starting Point (Phase 8.2)

When ready to build the agent, use this as your base prompt:

```
Create a C# console application called InventoryImportAgent in the 
SportsCardStore solution. It should:

1. Read card data from an Excel file (Sports_Cards_in_Order.xlsx) 
   using ClosedXML
2. Map Excel columns to SportsCard entity fields per the schema in 
   docs/INVENTORY_IMPORT_SCHEMA.md
3. Validate each row — skip blanks, enforce Raw cards have null Grade,
   prices > 0, year range 1800-2100
4. Call POST /api/sportscards for each valid card using HttpClient
5. Log results: how many imported, skipped, and failed
6. Accept the API base URL and Excel file path as command line arguments

Use ILogger for structured logging. Include error handling that 
continues processing remaining rows if a single row fails.
```

---

*Last Updated: April 2026 — Created in preparation for Phase 8.2*
