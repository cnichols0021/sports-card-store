# Sports Card Store — AI Prompt Engineering Log
> **Purpose:** Living document tracking every prompt used to build the Sports Card Store project via GitHub Copilot and Claude. Update this after every session. This document serves as a reference, a portfolio artifact, and a learning record.

---

## How to Use This Document

- **Add every prompt you run** — even the ones that didn't work well
- **Note the tool used** (Copilot Chat, Copilot Inline, Claude, Azure MCP, Playwright MCP)
- **Rate the output** (✅ Great / ⚠️ Needed Tweaking / ❌ Off Track)
- **Log what you changed** if you had to correct or refine the output
- **Keep the "Lessons Learned" section updated** — this becomes your interview talking points

---

## Project Context

| Item | Detail |
|---|---|
| **Project Name** | Sports Card Store |
| **Stack** | ASP.NET Core 10, Azure SQL, Azure Blob Storage, Azure App Service, Stripe |
| **Architecture** | Clean Architecture (API / Core / Infrastructure / Tests) |
| **Primary AI Tools** | GitHub Copilot (VS Code), Claude, Azure MCP Server, Playwright MCP |
| **Goal** | Portfolio project demonstrating AI-assisted full-stack development |
| **Started** | April 2026 |

---

## Phase 0 — Project Planning

### Prompt 0.1 — Project Plan Generation
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Help me write a PROJECT_PLAN.md for an ASP.NET Core 8 ecommerce 
site for selling sports cards on Azure. Include sections for: 
project overview, entity list with fields, Azure services needed, 
API endpoint plan, AI agents planned, and tech stack decisions. 
I want clean architecture with separate API, Core, Infrastructure, 
and Tests projects. I'll use Azure SQL, Blob Storage, App Service, 
Stripe for payments, and GitHub Copilot for AI-assisted development.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Copilot produced a 7.5/10 first draft — better than average for a single prompt
  - Smartly split `Card` and `CardListing` into separate entities (correct marketplace architecture) without being asked
  - Added `Brand` and `Category` as normalized entities unprompted
  - Added `MediatR`, `FluentValidation`, `AutoMapper` to tech stack — all solid real-world choices
  - Added `SportsCardStore.Shared` as a fifth project beyond the four requested — kept it
  - **Required corrections (see Phase 0.2):** Azure SQL tier too expensive, GitHub Actions instead of Azure Pipelines, Azure AD B2C not phased correctly, QnA Maker deprecated, missing personal infrastructure section, generic AI agents instead of specific hands-on builds

---

## Phase 0.2 — Plan Refinement

### Prompt 0.2.1 — Fix Azure Services (Cost & CI/CD)
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
In the PROJECT_PLAN.md Azure Services section, make these changes:
1. Replace Azure SQL "General Purpose tier" with Basic/Standard S0 tier 
   (~$5-15/month), appropriate for development and portfolio projects.
2. In DevOps & Deployment, replace "GitHub Actions" with "Azure Pipelines" 
   as the CI/CD tool.
3. Move Docker to a "Future Consideration" note rather than an active 
   planned service.
Keep all other Azure services as-is.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - All three changes applied cleanly with no issues

---

### Prompt 0.2.2 — Fix Authentication Approach
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
In the PROJECT_PLAN.md Tech Stack Decisions section under Authentication 
& Authorization, add a note that Azure AD B2C is a future enhancement. 
The initial implementation will use ASP.NET Core Identity with JWT Bearer 
Tokens only. This keeps Phase 1 complexity manageable for a portfolio 
project.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Tech Stack section correctly split into Phase 1 and Future Enhancements
  - Note: Azure Services section still listed AD B2C without "future" qualifier — required one additional follow-up prompt (0.2.7)

---

### Prompt 0.2.3 — Fix Deprecated QnA Maker Reference
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
In the PROJECT_PLAN.md AI Agents section under Customer Service Bot, 
replace "QnA Maker" with "Azure AI Language" as QnA Maker has been 
deprecated by Microsoft. Also add an alternative option of calling 
the Claude or OpenAI API directly as a simpler implementation path.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Clean replacement, alternative Claude/OpenAI path added as requested

---

### Prompt 0.2.4 — Add Personal Infrastructure Section
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Add a new section to PROJECT_PLAN.md called "Personal Infrastructure 
and Data Assets" after the Project Overview section. Include:
- Existing card inventory tracked in a detailed Excel spreadsheet 
  (primary data source for the Inventory Import Agent)
- Epson ES-400 II duplex scanner for card image digitization 
  (drives the image upload pipeline design)
- Active WhatNot card breaking business (future integration 
  consideration for live selling features)
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Section placed correctly after Project Overview
  - Copilot expanded each bullet with good context around scanning pipeline and WhatNot market insights

---

### Prompt 0.2.5 — Replace Generic AI Agents with Specific Hands-On Builds
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
In PROJECT_PLAN.md replace the generic AI Agents Planned section with 
these three specific hands-on agent builds that will be actively 
developed as part of this project:

1. Card Listing Agent
   - Purpose: Accept a raw card description string and generate a 
     complete optimized product listing
   - Technology: C# class using HttpClient to call Claude API
   - Output: Structured CardListing object with Title, Description, 
     SuggestedPrice, Tags, and Category
   - Learning Goal: Prompt pipelines, structured output parsing, 
     calling AI APIs from C#

2. Inventory Import Agent
   - Purpose: Process existing Excel card tracker and import cards 
     into the database
   - Technology: C# with EPPlus or ClosedXML for Excel parsing, 
     maps columns to CardListing entities
   - Output: Validated SQL inserts or API calls to populate the database
   - Learning Goal: File parsing, multi-step agent chains, tool use 
     and function calling

3. Price Research Agent
   - Purpose: Research recent sold listings and return a suggested 
     price range for a given card
   - Technology: C# agent with web search integration
   - Output: Price range recommendation with market data sources
   - Learning Goal: Agents with web search tools, ReAct-style 
     reasoning loops, grounding AI output in real data

Keep the future AI features (Card Recognition, Recommendation Engine, 
Customer Service Bot) as a separate "Future AI Features" subsection.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - All three agents documented with correct technology, output, and learning goals
  - Future AI features cleanly separated into their own subsection
  - GitHub Copilot Integration section retained above the hands-on builds

---

### Prompt 0.2.6 — Add Azure MCP to Development Workflow
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Add a new subsection to the Tech Stack Decisions section called 
"AI-Assisted Development Tools" with the following:

- GitHub Copilot: Primary AI coding assistant in VS Code and 
  Visual Studio 2022
- Azure MCP Server: Used in Copilot Agent Mode to create and 
  configure Azure resources directly from VS Code chat. Configured 
  via .vscode/mcp.json using @azure/mcp package.
- Playwright MCP: Browser automation of Azure Portal for 
  verification and GUI tasks not covered by Azure MCP
- Claude: Architecture planning, prompt refinement, and code review
- Prompt Engineering Log: Living document tracking all prompts used, 
  output ratings, and lessons learned throughout development
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Section added cleanly to Tech Stack Decisions
  - GitHub Copilot appears in both Development Tools and AI-Assisted Development Tools — minor duplication, not worth correcting

---

### Prompt 0.2.7 — Fix AD B2C in Azure Services Section
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
In PROJECT_PLAN.md under the Azure Services Needed section, under 
Authentication & Security, add a note to Azure Active Directory B2C 
marking it as a "Future Enhancement" consistent with the Tech Stack 
Decisions section. Phase 1 will use ASP.NET Core Identity and JWT 
only. Azure AD B2C will be added in a later phase for social logins.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Needed because Prompt 0.2.2 fixed Tech Stack but left Azure Services section out of sync

---

## Phase 0.3 — Solution Cleanup & .NET Upgrade

### Prompt 0.3.1 — Remove WeatherForecast Placeholder Files
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
I have a default ASP.NET Core 8 Web API project that still contains 
the WeatherForecast.cs placeholder file and its controller. Remove 
both files and update Program.cs to remove any references to them. 
The project is SportsCardStore.API.
```
- **Output Rating:** ⚠️ Needed Tweaking
- **Notes / What Was Changed:**
  - First attempt did not remove the files — WeatherForecast.cs and WeatherForecastController.cs remained in the repo after merge
  - Required a second targeted prompt to finish the job (see 0.3.2)

---

### Prompt 0.3.2 — Remove WeatherForecast Files (Follow-up)
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Delete these two files from the SportsCardStore.API project:
1. WeatherForecast.cs (in the root of the API project)
2. Controllers/WeatherForecastController.cs

The Program.cs is already clean with no references to these files 
so no other changes are needed. Just delete both files.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Both files removed cleanly on second attempt
  - Controllers folder disappeared automatically since it was empty after deletion

---

### Prompt 0.3.3 — Remove Placeholder Class1.cs Files
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Remove all placeholder files from the solution: Class1.cs from 
SportsCardStore.Core, SportsCardStore.Infrastructure, and 
SportsCardStore.Shared, and UnitTest1.cs from all test projects. 
These are default template files and should be deleted before 
real code is added.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - All placeholder files removed cleanly
  - Extra SportsCardStore.Tests project also removed in this pass

---

### Prompt 0.3.4 — Upgrade Solution to .NET 10
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Update all .csproj files in this solution from net8.0 to net10.0. 
This includes all projects in the src folder (SportsCardStore.API, 
SportsCardStore.Core, SportsCardStore.Infrastructure, 
SportsCardStore.Shared) and all projects in the tests folder 
(SportsCardStore.Tests, SportsCardStore.UnitTests, 
SportsCardStore.IntegrationTests, SportsCardStore.FunctionalTests). 
Also update the Swashbuckle.AspNetCore NuGet package reference in 
the API project to the latest version compatible with .NET 10. 
After updating, run dotnet restore to confirm packages resolve 
correctly.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - All src and test projects updated to net10.0 cleanly
  - Swashbuckle updated to 7.0.0
  - Copilot proactively added EF Core SqlServer 10.0.0 and EF Core Tools 10.0.0 to Infrastructure — kept these
  - UnitTests project came back with xUnit, Moq, FluentAssertions, coverlet already wired up
  - Decision: .NET 10 chosen because it is the current LTS (released November 2025)

---

## Phase 1 — Solution Scaffolding & Data Model

### Prompt 1.1 — Solution Structure
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
I'm building an ASP.NET Core 8 Web API ecommerce site for selling 
sports cards hosted on Azure. Set up the solution structure with 
separate projects for: API, Core (models/interfaces), Infrastructure 
(EF Core/data access), and Tests. Use clean architecture principles. 
Show me the folder structure and the .csproj references I need.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - All four src projects created correctly with proper project references
  - Copilot added SportsCardStore.Shared as a fifth project — kept it
  - All test projects scaffolded with correct xUnit setup

---

### Prompt 1.2 — SportsCard Entity Model
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Create an Entity Framework Core entity model called SportsCard with 
these fields: Id (int), PlayerName, Year (int), Brand, CardNumber, 
Sport, Team, Grade (decimal), GradingCompany (PSA/BGS/SGC/Raw), 
Condition, Price (decimal), Quantity (int), ImageUrl, Description, 
IsAvailable (bool), CreatedDate, UpdatedDate. Add data annotations 
for validation and include a Category enum for Baseball, Football, 
Basketball, Hockey. Put this in the Core project.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Created `Entities/` and `Enums/` subfolder structure unprompted
  - XML doc comments on every property
  - `Grade` made nullable — raw cards don't have a grade
  - `[Url]` validation on ImageUrl, range on Year (1800-2100)
  - Correct decimal precision: `decimal(3,1)` for Grade, `decimal(8,2)` for Price
  - **Note:** Entity uses `int Id` — project plan specifies `Guid`. Flag when building DbContext

---

### Prompt 1.3 — DbContext & EF Configuration
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Create an AppDbContext using Entity Framework Core for the SportsCard 
entity. Configure it to use Azure SQL. Use Fluent API in a separate 
EntityTypeConfiguration class for SportsCard. Add appropriate indexes 
on PlayerName and Year. Put the DbContext in the Infrastructure project.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Created `Data/` and `Data/Configurations/` subfolder structure unprompted
  - Auto-updates timestamps in SaveChanges/SaveChangesAsync — production-grade pattern added unprompted
  - Prevents CreatedDate from being modified on update
  - OnConfiguring fallback for EF design-time tooling
  - Full Fluent API with explicit SQL types for every property
  - Enum conversions to int, HasDefaultValueSql("GETUTCDATE()") on date fields
  - Six indexes added (vs two requested) — PlayerName, Year, composite, Sport, IsAvailable, CreatedDate
  - **Minor issue:** Duplicate config application via ApplyConfiguration + ApplyConfigurationsFromAssembly

---

### Prompt 1.4 — Seed Data
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Create an EF Core data seeder class that seeds 10 realistic sports 
cards into the SportsCard table. Use real players across baseball, 
football, and basketball. Include a mix of graded (PSA, BGS) and raw 
cards with realistic prices between $5 and $500. Wire it up to run 
on app startup in development environment only.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Created `Data/Seeders/` subfolder unprompted
  - Idempotent guard prevents double-seeding
  - 10 realistic cards across baseball, football, basketball — all three grading companies plus Raw
  - Raw cards correctly have `Grade = null`
  - Prices $45-$495, accurate card details
  - Program.cs wired up correctly with DI, development-only block, exception handling

---

## Phase 2 — Azure Infrastructure Setup

### Prompt 2.1 — Azure Resource Group & Core Services
- **Tool:** Azure MCP Server (Copilot Agent Mode)
- **Date:** April 2026
- **Prompt:**
```
Using my Azure subscription, create a Resource Group called 
'sports-card-store-rg' in East US. Then create an Azure SQL Server 
called 'sportscard-sql-server' with a Basic tier database called 
'SportscardDb'. Create an App Service Plan (Basic B1) and a Web App 
called 'sportscard-api'. Return the connection strings I need.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Resource Group in East US, SQL Server in Central US (capacity fallback) ✅
  - Azure SQL `SportscardDb` at Basic tier (~$5/month) ✅
  - App Service Plan B1 (~$13/month), Web App at https://sportscard-api.azurewebsites.net ✅
  - **Security issue:** Full connection string with password written directly into `appsettings.json` and `AZURE_DEPLOYMENT_SUMMARY.md` — both committed to public repo. Required immediate remediation (see below)

---

### Phase 2 Security Remediation — Credentials Exposed in Public Repo
- **Date:** April 2026
- **What Happened:** Full Azure SQL password committed to public repo in `appsettings.json` and `AZURE_DEPLOYMENT_SUMMARY.md`
- **Actions Taken:**
  - Azure SQL password rotated immediately
  - Both files sanitized and re-committed by Claude via GitHub MCP
  - `.gitignore` updated with broad credential file pattern coverage including `azure-credentials.json`

---

### Prompt 2.3 — Set Up User Secrets for Local Development
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Set up .NET user secrets for the SportsCardStore.API project to 
store the Azure SQL connection string securely for local development.

1. Run: dotnet user-secrets init --project src/SportsCardStore.API
2. Run: dotnet user-secrets set "ConnectionStrings:DefaultConnection" 
   "PLACEHOLDER" --project src/SportsCardStore.API
3. Update Program.cs to read from user secrets in Development and 
   Azure App Service Configuration in Production.
4. Show me where the user secrets file lives locally.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - User secrets initialized, connection string stored locally (never committed)
  - Windows location: `%APPDATA%\Microsoft\UserSecrets\<guid>\secrets.json`

---

### Prompt 2.4 — MigrateAsync and InitialCreate Migration
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Replace EnsureCreatedAsync() with MigrateAsync() in Program.cs.
Create migration: dotnet ef migrations add InitialCreate 
  --project src/SportsCardStore.Infrastructure 
  --startup-project src/SportsCardStore.API
Apply migration: dotnet ef database update 
  --project src/SportsCardStore.Infrastructure 
  --startup-project src/SportsCardStore.API
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - `EnsureCreatedAsync()` replaced with `MigrateAsync()` ✅
  - Migration `20260404180525_InitialCreate` generated — all columns, types, and 6 indexes correct ✅
  - All three migration files created correctly ✅
  - Clean `Down()` method included ✅
  - Migration applied to Azure SQL — `SportscardDb` table is live ✅

---

### Prompt 2.2.1 — Create Azure Blob Storage Account
- **Tool:** Azure MCP Server (Copilot Agent Mode)
- **Date:** April 2026
- **Prompt:**
```
Using my Azure subscription and the existing resource group 
'sports-card-store-rg', create an Azure Blob Storage account with:
- Storage account name: sportscardstore (or similar available name)
- Region: Central US (match existing resources)
- Performance: Standard
- Redundancy: LRS
- Create a private container called 'card-images'

IMPORTANT: Do NOT write the storage connection string or access 
keys into any file. Return in chat only so I can store manually 
in user secrets.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Blob Storage account created successfully ✅
  - `card-images` container created ✅
  - Connection string returned in chat only — credentials did NOT enter any file ✅
  - Security reminder in the prompt was respected — confirms that explicit security instructions in Azure prompts work

---

### Prompt 2.2.2 — Add BlobStorageService to Infrastructure Project
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Add Azure Blob Storage support to SportsCardStore.Infrastructure.
1. Add NuGet package: Azure.Storage.Blobs
2. Create IBlobStorageService interface in Core/Interfaces/ with:
   - UploadImageAsync(Stream, string) returns Task<string>
   - DeleteImageAsync(string) returns Task<bool>
   - GetImageUrlAsync(string) returns Task<string>
3. Create BlobStorageService.cs in Infrastructure/Services/ 
   implementing the interface. Read connection string from 
   "AzureBlobStorage:ConnectionString" and container name from 
   "AzureBlobStorage:ContainerName". Include ILogger logging.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Created `Core/Interfaces/` folder unprompted — correct clean architecture placement ✅
  - `Azure.Storage.Blobs 12.27.0` added to Infrastructure.csproj ✅
  - Constructor validates both config values on startup — fail fast pattern ✅
  - `UploadImageAsync` generates unique filename using timestamp + Guid to prevent collisions ✅
  - Resets stream position before upload ✅
  - `GetContentType()` helper covers jpg, png, gif, bmp, webp, tiff ✅
  - `DeleteImageAsync` uses `DeleteIfExistsAsync` — won't throw on missing file ✅
  - `GetImageUrlAsync` checks existence before returning URL ✅
  - XML doc comments on interface methods ✅
  - No credentials anywhere in the generated code ✅

---

### Prompt 2.2.3 — Register BlobStorageService in Program.cs
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Register BlobStorageService in Program.cs:
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
Add required using statements. Do not add any credentials to appsettings.json.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Service registered correctly with proper using statements ✅
  - `appsettings.json` stayed clean — no credentials added ✅
  - Security lesson from Phase 2.1 successfully carried forward ✅

---

## Phase 3 — API Layer

### Prompt 3.1 — SportsCards Controller
- **Tool:** GitHub Copilot Chat
- **Date:**
- **Prompt:**
```
Create a SportsCardsController in ASP.NET Core 8 with these endpoints:
GET /api/cards (all cards, with optional query params for sport, 
brand, grading company, min/max price), GET /api/cards/{id}, 
POST /api/cards, PUT /api/cards/{id}, DELETE /api/cards/{id}. 
Use a service interface (ISportsCardService) and inject it. 
Return proper HTTP status codes. Include basic pagination support 
on the GET all endpoint.
```
- **Output Rating:** ⬜ Pending
- **Notes / What Was Changed:**

---

### Prompt 3.2 — Unit Tests
- **Tool:** GitHub Copilot Chat
- **Date:**
- **Prompt:**
```
Generate xUnit unit tests for SportsCardService covering: GetAll 
with filters applied, GetById returns null for missing id, Create 
validates required fields, and Update returns 404 for missing card. 
Use Moq for mocking the repository. Follow Arrange/Act/Assert pattern 
and add XML doc comments explaining what each test validates.
```
- **Output Rating:** ⬜ Pending
- **Notes / What Was Changed:**

---

## Phase 4 — Frontend

*(Prompts to be added as this phase begins)*

---

## Phase 5 — Stripe Payments

*(Prompts to be added as this phase begins)*

---

## Phase 6 — Image Upload Pipeline

*(Prompts to be added as this phase begins)*

---

## Phase 7 — Admin Panel

*(Prompts to be added as this phase begins)*

---

## Phase 8 — AI Agents

### Prompt 8.1 — Card Listing Agent (Scaffold)
- **Tool:** GitHub Copilot Chat
- **Date:**
- **Prompt:**
```
Create a C# class called CardListingAgent that accepts a raw card 
description string, calls the Anthropic Claude API using HttpClient, 
and returns a structured CardListing object with Title, Description, 
SuggestedPrice, Tags, and Category. Use System.Text.Json for 
deserialization. Include error handling and logging via ILogger.
```
- **Output Rating:** ⬜ Pending
- **Notes / What Was Changed:**

---

### Prompt 8.2 — Inventory Import Agent
- **Tool:** *(to be determined)*
- **Date:**
- **Prompt:** *(to be filled in)*
- **Output Rating:** ⬜ Pending
- **Notes / What Was Changed:**

---

### Prompt 8.3 — Price Research Agent
- **Tool:** *(to be determined)*
- **Date:**
- **Prompt:** *(to be filled in)*
- **Output Rating:** ⬜ Pending
- **Notes / What Was Changed:**

---

## CI/CD Pipeline

### Prompt CI.1 — Azure Pipelines YAML Pipeline
- **Tool:** GitHub Copilot Chat
- **Date:**
- **Prompt:** *(to be filled in when ready)*
- **Output Rating:** ⬜ Pending
- **Notes / What Was Changed:**

---

## Lessons Learned

> *(Update this section regularly — these become your interview talking points)*

| # | Lesson | Phase It Came From |
|---|---|---|
| 1 | Copilot defaults to enterprise/production-scale answers — always review cost and complexity assumptions | Phase 0 |
| 2 | Copilot made smart architectural decisions unprompted (Card/CardListing split) — don't always override AI output | Phase 0 |
| 3 | A single prompt fix doesn't cascade — always check related sections after targeted changes | Phase 0.2 |
| 4 | When asking Copilot to delete files, name them explicitly | Phase 0.3 |
| 5 | Copilot proactively added EF Core packages to Infrastructure when upgrading .NET — it reads context and anticipates next steps | Phase 0.3 |
| 6 | Copilot creates correct subfolder structure unprompted when the project signals clean architecture intent | Phase 1 |
| 7 | Always verify PK type consistency early — int vs Guid cascades through many files | Phase 1 |
| 8 | Copilot adds timestamp auto-update in SaveChanges unprompted when entities have date fields | Phase 1 |
| 9 | Copilot exceeds index scope (6 vs 2 requested) — review generated indexes, extras have storage/write costs | Phase 1 |
| 10 | EnsureCreatedAsync() bypasses EF migrations — always use MigrateAsync() with a real database | Phase 1/2 |
| 11 | **Critical:** AI-assisted Azure setup writes credentials into config files — always review appsettings.json before merging | Phase 2 |
| 12 | Exposed credentials in public repos must be rotated immediately — bots scan GitHub continuously | Phase 2 |
| 13 | Use dotnet user-secrets for local dev, Azure App Service Configuration for production — never appsettings.json | Phase 2 |
| 14 | Add credential file patterns to .gitignore proactively before creating those files | Phase 2 |
| 15 | Explicitly including a security reminder in Azure Blob Storage prompts ("Do NOT write to any file") was respected — the lesson from Phase 2.1 carried forward successfully | Phase 2 |

---

## Prompt Patterns That Worked Well

- Numbered list of specific changes in one prompt produces clean targeted edits (Prompt 0.2.1)
- Providing technology, output shape, and learning goal in agent prompts produces detailed documentation
- Naming files explicitly when asking for deletions (Prompt 0.3.2)
- Entity model prompts with field types, validation rules, and target project = production-quality output
- DbContext prompts specifying Fluent API + separate configuration class = well-structured EF config
- Seed data prompts specifying real player names and grading company mix = accurate domain data
- Migration prompts including both `add` and `update` commands = complete runnable workflow
- **Including explicit security instructions in Azure prompts ("Do NOT write credentials to any file") prevents credential exposure**

---

## Prompt Patterns That Didn't Work

- Broad prompts touching multiple sections miss consistency issues — review related sections after changes
- Vague delete prompts don't reliably delete files — name files explicitly with full paths
- Azure infrastructure prompts without security reminders will write connection strings to config files — always include the security instruction

---

*Last Updated: April 2026*
