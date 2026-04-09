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
| Bowman First | `IsBowmanFirst` | bool | true/false or yes/no in Excel. Marks the first Bowman card of a prospect — significantly higher value than other early cards. |
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
| Bowman First | High-value attribute frequently searched by collectors — first Bowman card of a prospect commands significant premium |
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

## IsBowmanFirst — Notes for the Import Agent

- Only applies to Bowman brand cards (Brand = "Bowman", "Bowman Chrome", "Bowman Draft", etc.)
- Default to `false` if column is blank or missing
- Bowman First Chrome 1st Edition (blue label) are the most sought-after — this flag captures all variants
- Useful for filtering: buyers frequently search specifically for `IsBowmanFirst = true`
- Future consideration: add this as a query param to `GET /api/sportscards` filter options

---

## Import Agent Notes

- **Excel file location:** `Sports_Cards_in_Order.xlsx` (approximately 477 cards)
- **Recommended library:** ClosedXML for Excel parsing in C#
- **Validation rules:**
  - Skip rows where PlayerName is blank
  - Raw cards must have Grade = null — do not import a grade value for Raw cards
  - Prices must be > 0
  - Year must be between 1800 and 2100
  - Default IsAvailable = true if column is blank
  - Default IsBowmanFirst = false if column is blank
- **Import strategy:** Call the existing `POST /api/sportscards` endpoint rather than writing directly to the database — keeps the import agent decoupled from the data layer and exercises the real API
- **Duplicate handling:** Check if a card with the same PlayerName + Year + Brand + CardNumber already exists before inserting

---

## Copilot Prompt — Add IsBowmanFirst to Entity, DTOs, and Migration

Run these prompts in Copilot to add `IsBowmanFirst` across the full stack:

**Prompt 1 — Add to SportsCard entity and Shared DTOs:**
```
Add a new boolean field called IsBowmanFirst to the SportsCard entity
in SportsCardStore.Core/Entities/SportsCard.cs. Add an XML doc comment
explaining it marks the first Bowman card of a prospect which commands
a significant price premium. Default value should be false.

Also add IsBowmanFirst (bool, default false) to the
CreateSportsCardRequest and UpdateSportsCardRequest in
SportsCardStore.Shared/Models/SportsCardDtos.cs, and add it to
SportsCardResponse as well.
```

**Prompt 2 — Add to migration:**
```
Create an EF Core migration called AddInventoryFields to add these
new columns to the SportsCards table:

- SetName (nvarchar(100), not null)
- IsRookie (bit, not null, default 0)
- IsAutograph (bit, not null, default 0)
- IsRelic (bit, not null, default 0)
- IsBowmanFirst (bit, not null, default 0)

Also update SportsCardConfiguration.cs in Infrastructure to include
Fluent API configuration for all five new fields.

dotnet ef migrations add AddInventoryFields \
  --project src/SportsCardStore.Infrastructure \
  --startup-project src/SportsCardStore.API

dotnet ef database update \
  --project src/SportsCardStore.Infrastructure \
  --startup-project src/SportsCardStore.API
```

**Prompt 3 — Add to controller filter params:**
```
Add IsBowmanFirst as an optional query parameter filter to the
GetAllCards endpoint in SportsCardsController.cs, and add it
to the GetAllAsync method signature in ISportsCardService.cs
and the SportsCardService.cs implementation. When IsBowmanFirst
is true, filter to only return cards where IsBowmanFirst = true.
```

**Prompt 4 — Update InventoryImportAgent:**
```
Update the MapRowToSportsCard method in
src/InventoryImportAgent/Program.cs to read the "Bowman First"
column from Excel and map it to the IsBowmanFirst field on
CreateSportsCardRequest. Default to false if the column is
missing or blank.
```

---

*Last Updated: April 2026 — IsBowmanFirst added*
