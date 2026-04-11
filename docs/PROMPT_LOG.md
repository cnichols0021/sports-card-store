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

| Item                 | Detail                                                                    |
| -------------------- | ------------------------------------------------------------------------- |
| **Project Name**     | Sports Card Store                                                         |
| **Stack**            | ASP.NET Core 10, Azure SQL, Azure Blob Storage, Azure App Service, Stripe |
| **Architecture**     | Clean Architecture (API / Core / Infrastructure / Tests)                  |
| **Primary AI Tools** | GitHub Copilot (VS Code), Claude, Azure MCP Server, Playwright MCP        |
| **Goal**             | Portfolio project demonstrating AI-assisted full-stack development        |
| **Started**          | April 2026                                                                |

---

## Phase 0 — Project Planning

### Prompt 0.1 — Project Plan Generation

- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** 7.5/10 first draft. Smart entity splits, good tech stack choices. Required 7 follow-up corrections (see Phase 0.2).

---

## Phase 0.2 — Plan Refinement

### Prompt 0.2.1 — Fix Azure Services (Cost & CI/CD)

- **Output Rating:** ✅ Great — All three changes applied cleanly

### Prompt 0.2.2 — Fix Authentication Approach

- **Output Rating:** ✅ Great — Tech Stack split Phase 1 / Future. Azure Services required follow-up (0.2.7)

### Prompt 0.2.3 — Fix Deprecated QnA Maker Reference

- **Output Rating:** ✅ Great — Replaced with Azure AI Language + Claude/OpenAI alternative

### Prompt 0.2.4 — Add Personal Infrastructure Section

- **Output Rating:** ✅ Great — Excel inventory, Epson scanner, WhatNot business added

### Prompt 0.2.5 — Replace Generic AI Agents with Specific Hands-On Builds

- **Output Rating:** ✅ Great — Three specific agents with technology/output/learning goals

### Prompt 0.2.6 — Add Azure MCP to Development Workflow

- **Output Rating:** ✅ Great — AI-Assisted Development Tools section added

### Prompt 0.2.7 — Fix AD B2C in Azure Services Section

- **Output Rating:** ✅ Great — Fixed consistency gap between Tech Stack and Azure Services

---

## Phase 0.3 — Solution Cleanup & .NET Upgrade

### Prompt 0.3.1 — Remove WeatherForecast Placeholder Files

- **Output Rating:** ⚠️ Needed Tweaking — First attempt failed. Required explicit follow-up (0.3.2)

### Prompt 0.3.2 — Remove WeatherForecast Files (Follow-up)

- **Output Rating:** ✅ Great — Named files explicitly, both removed cleanly

### Prompt 0.3.3 — Remove Placeholder Class1.cs Files

- **Output Rating:** ✅ Great — All placeholder files removed, extra test project also removed

### Prompt 0.3.4 — Upgrade Solution to .NET 10

- **Output Rating:** ✅ Great — All projects net10.0, EF Core 10 added proactively, full test stack included

---

## Phase 1 — Solution Scaffolding & Data Model

### Prompt 1.1 — Solution Structure

- **Output Rating:** ✅ Great — All projects created with correct references

### Prompt 1.2 — SportsCard Entity Model

- **Output Rating:** ✅ Great — Entities/Enums subfolders, XML docs, nullable Grade, correct precision

### Prompt 1.3 — DbContext & EF Configuration

- **Output Rating:** ✅ Great — Timestamp auto-update, 6 indexes, full Fluent API. Minor: duplicate ApplyConfiguration calls

### Prompt 1.4 — Seed Data

- **Output Rating:** ✅ Great — 10 real players, idempotent guard, all grading companies, Program.cs wired correctly

---

## Phase 2 — Azure Infrastructure Setup

### Prompt 2.1 — Azure Resource Group & Core Services

- **Output Rating:** ✅ Great — Resources created. **Security issue:** credentials committed to public repo. Required immediate remediation.

### Phase 2 Security Remediation

- Password rotated, files sanitized by Claude via GitHub MCP, .gitignore updated

### Prompt 2.3 — User Secrets Setup

- **Output Rating:** ✅ Great — Secrets initialized locally, never committed

### Prompt 2.4 — MigrateAsync + InitialCreate Migration

- **Output Rating:** ✅ Great — MigrateAsync in place, migration applied to Azure SQL, all 6 indexes in migration

### Prompt 2.2.1 — Create Azure Blob Storage Account

- **Output Rating:** ✅ Great — Storage + container created. Credentials in chat only, security instruction respected.

### Prompt 2.2.2 — Add BlobStorageService to Infrastructure

- **Output Rating:** ✅ Great — Core/Interfaces/ created, fail-fast validation, unique filename generation, no credentials

### Prompt 2.2.3 — Register BlobStorageService in Program.cs

- **Output Rating:** ✅ Great — Registered correctly, appsettings.json stayed clean

---

## Phase 3 — API Layer

### Prompt 3.1 — SportsCards Controller, Service, and DTOs

- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Full CRUD with 7 filter params and pagination ✅
  - ISportsCardService + PagedResult<T> in Core/Interfaces ✅
  - SportsCardService with smart IsAvailable default filter ✅
  - Three DTOs + PagedSportsCardResponse in API/Models ✅
  - ProducesResponseType on all endpoints, CreatedAtAction on POST ✅
  - Entity → DTO mapping (never returns raw entities) ✅
  - ILogger + exception handling throughout ✅
  - Minor: redundant timestamp set in controller (AppDbContext already handles this)
  - Minor: PagedResult<T> in ISportsCardService.cs — should be its own file

### Prompt 3.2 — Unit Tests

- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - 12 tests generated — 2 bonus tests beyond the 10 requested ✅
  - Controllers/ subfolder, all four src projects referenced in csproj ✅
  - Test naming: MethodName_Condition_ExpectedResult ✅
  - Verify() calls confirm Delete service interactions ✅
  - Status codes AND body values validated on every test ✅

---

## Phase 4 — Frontend

_(Prompts to be added as this phase begins)_

---

## Phase 5 — Stripe Payments

_(Prompts to be added as this phase begins)_

---

## Phase 6 — Image Upload Pipeline

_(Prompts to be added as this phase begins)_

---

## Phase 7 — Admin Panel

_(Prompts to be added as this phase begins)_

---

## Phase 8 — AI Agents

### Prompt 8.2 — Inventory Import Agent Scaffold

- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Created `src/InventoryImportAgent/` as a standalone C# console app ✅
  - ClosedXML for Excel parsing, dynamic column mapping via header row ✅
  - All required fields validated, Raw cards enforced to have null Grade ✅
  - ParseBoolean handles true/false/yes/no/1/y ✅
  - Posts to `POST /api/sportscards` via HttpClient — decoupled from data layer ✅
  - Imported / Skipped / Failed counts logged, API URL + file path as args ✅
  - DTOs correctly moved to Shared project unprompted — strong architectural decision ✅

### Prompt 8.2.1 — Add Entity Fields + Migration

- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** SetName, IsRookie, IsAutograph, IsRelic added to entity. Migration `20260409113542_AddInventoryFields` applied to Azure SQL ✅

### Prompt 8.2.2 — Add IsBowmanFirst Field

- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - IsBowmanFirst added to entity, all Shared DTOs, ISportsCardService filter param, and controller GET query param ✅
  - Separate migration `20260409114438_AddIsBowmanFirstField` applied ✅

### Prompt 8.2.3 — Fix InventoryImportAgent IsBowmanFirst Mapping

- **Tool:** Claude (direct GitHub push)
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Copilot hallucinated completion twice — SHA was unchanged both times
  - Claude pushed the fix directly: `isBowmanFirst = ParseBoolean(GetCellValue(..., "Bowman First"))` and `IsBowmanFirst = isBowmanFirst` in request initializer ✅

### Prompt 8.2.4 — Fix Controller CreateCard/UpdateCard Missing Field Mappings

- **Tool:** Claude (direct GitHub push)
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - All new fields mapped in CreateCard entity initializer and UpdateCard entity update block ✅
  - Claude added `MapToResponse()` private helper — all endpoints now return consistent, complete response DTOs ✅

### Prompt 8.2.5 — Add Parallel Fields (ParallelName + PrintRun)

- **Tool:** Claude (direct GitHub push)
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Added `ParallelName` (string?, max 100) and `PrintRun` (int?, range 1-10000) to entity, all Shared DTOs, service filters, controller query params, and InventoryImportAgent ✅
  - Service filter: `parallelName` does case-insensitive contains search; `maxPrintRun` returns all cards numbered at or below the value ✅
  - InventoryImportAgent reads "Parallel" and "Print Run" columns — both nullable, base cards leave blank ✅
  - Migration `20260409204603_AddParallelFields` generated and applied to Azure SQL ✅
  - All 6 files pushed in a single commit ✅
  - **Origin:** User identified parallels as a key value driver in the hobby — both numbered (/25, /10, 1/1) and non-numbered (Gold, Sky Blue) variants needed separate fields to enable filtering and pricing research

### Prompt 8.2.6 — Default Condition to "Near Mint or Better"

- **Tool:** Claude (direct GitHub push)
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Added `private const string DefaultCondition = "Near Mint or Better"` to InventoryImportAgent ✅
  - Condition column now falls back to that constant when blank in Excel ✅
  - Same default applied by Claude when parsing dictated card data in chat ✅
  - **Origin:** User uses Claude Desktop voice dictation to enter card data — skipping blank optional fields is essential for efficient dictation workflow

---

## Current Excel Column Schema (21 columns)

| #   | Column          | Required | Default                       |
| --- | --------------- | -------- | ----------------------------- |
| 1   | Player Name     | ✅       | —                             |
| 2   | Team            | ✅       | —                             |
| 3   | Brand           | ✅       | —                             |
| 4   | Set Name        | ✅       | —                             |
| 5   | Card Number     | ✅       | —                             |
| 6   | Year            | ✅       | —                             |
| 7   | Sport           | ✅       | —                             |
| 8   | Rookie          |          | false                         |
| 9   | Autograph       |          | false                         |
| 10  | Relic           |          | false                         |
| 11  | Bowman First    |          | false                         |
| 12  | Grading Company | ✅       | —                             |
| 13  | Card Grade      |          | null (blank for Raw)          |
| 14  | Condition       |          | Near Mint or Better           |
| 15  | Price           | ✅       | —                             |
| 16  | Quantity        | ✅       | —                             |
| 17  | Parallel        |          | null (blank for base)         |
| 18  | Print Run       |          | null (blank for non-numbered) |
| 19  | Description     |          | null                          |
| 20  | Image Url       |          | null                          |
| 21  | Is Available    |          | true                          |

---

### Prompt 8.1 — Card Listing Agent (Scaffold)

- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Complete CardListingAgent console application created ✅
  - Anthropic Claude API integration with proper authentication ✅
  - CardListing model added to Shared project with validation attributes ✅
  - Comprehensive error handling: HTTP errors, JSON parsing, API validation ✅
  - System.Text.Json deserialization with case-insensitive options ✅
  - ILogger integration with structured logging throughout ✅
  - Flexible API key configuration (environment variable + command line) ✅
  - Response cleaning for markdown-wrapped JSON responses ✅
  - CardListing validation with proper bounds checking ✅
  - Added to solution file with proper build configurations ✅
  - Generated comprehensive README.md with usage examples ✅
  - Follows Clean Architecture patterns established by InventoryImportAgent ✅

### Prompt 8.3 — Price Research Agent

- **Output Rating:** ⬜ Pending

---

## CI/CD Pipeline

### Prompt CI.1 — Azure Pipelines YAML Pipeline

- **Output Rating:** ⬜ Pending

---

## Lessons Learned

| #   | Lesson                                                                                                                                                                                              | Phase     |
| --- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------- |
| 1   | Copilot defaults to enterprise-scale answers — always review cost and complexity assumptions                                                                                                        | Phase 0   |
| 2   | Copilot makes smart architectural decisions unprompted — don't always override                                                                                                                      | Phase 0   |
| 3   | A single prompt fix doesn't cascade — check related sections after targeted changes                                                                                                                 | Phase 0.2 |
| 4   | Name files explicitly when asking Copilot to delete them                                                                                                                                            | Phase 0.3 |
| 5   | Copilot proactively adds packages when upgrading frameworks                                                                                                                                         | Phase 0.3 |
| 6   | Copilot creates correct subfolder structure unprompted when project signals clean architecture                                                                                                      | Phase 1   |
| 7   | Verify PK type consistency early — int vs Guid cascades through many files                                                                                                                          | Phase 1   |
| 8   | Copilot adds timestamp auto-update in SaveChanges unprompted when entities have date fields                                                                                                         | Phase 1   |
| 9   | Copilot exceeds index scope — review generated indexes, extras have storage/write costs                                                                                                             | Phase 1   |
| 10  | EnsureCreatedAsync() bypasses migrations — always use MigrateAsync() with a real database                                                                                                           | Phase 1/2 |
| 11  | **Critical:** Azure setup writes credentials into config files — review appsettings.json before every merge                                                                                         | Phase 2   |
| 12  | Exposed credentials must be rotated immediately — bots scan public repos continuously                                                                                                               | Phase 2   |
| 13  | Use dotnet user-secrets locally, Azure App Service Configuration for production                                                                                                                     | Phase 2   |
| 14  | Add credential file patterns to .gitignore before creating those files                                                                                                                              | Phase 2   |
| 15  | Explicit security instructions in Azure prompts were respected — lesson carried forward successfully                                                                                                | Phase 2   |
| 16  | Documentation files can also contain bad security guidance — review all generated docs, not just code                                                                                               | Phase 3   |
| 17  | Mock at the interface level (ISportsCardService) not the DB context — produces cleaner, faster unit tests                                                                                           | Phase 3   |
| 18  | Copilot adds bonus tests beyond what was requested — it infers missing scenarios from existing controller code                                                                                      | Phase 3   |
| 19  | When building a standalone agent that needs shared DTOs, Copilot correctly moves them to the Shared project — recognize and keep these unprompted architectural improvements                        | Phase 8   |
| 20  | Adding a new field to the entity doesn't automatically cascade to controller action mappings — always verify CreateCard/UpdateCard manually                                                         | Phase 8   |
| 21  | **Always verify by SHA, not by Copilot's confirmation** — if SHA hasn't changed after Copilot claims to have made a change, nothing happened                                                        | Phase 8   |
| 22  | A single `MapToResponse()` helper in the controller ensures all fields returned consistently — adding a field in future only requires one change                                                    | Phase 8   |
| 23  | Domain knowledge drives better data models — user's hobby expertise (parallel cards, print runs, Bowman Firsts) produced a richer schema than any generic sports card template would have           | Phase 8   |
| 24  | Voice dictation workflow (Claude Desktop mic → Claude parses → Excel row) eliminates manual data entry entirely — design data defaults around what's most commonly true to minimize required speech | Phase 8   |

---

## Prompt Patterns That Worked Well

- Numbered list of specific changes = clean targeted edits
- Technology + output shape + learning goal in agent prompts = detailed documentation
- Explicit file names when deleting = reliable results
- Entity prompts with field types + validation rules + target project = production-quality output
- Fluent API + separate configuration class in DbContext prompts = well-structured EF config
- Real player names + grading company mix in seed prompts = accurate domain data
- Both `add` and `update` migration commands in one prompt = complete runnable workflow
- Explicit security instructions in Azure prompts = no credential exposure
- Mocking at the interface level (ISportsCardService) = cleaner, faster, more maintainable tests
- Documenting the Excel column schema before prompting the import agent = precise output
- Always have Copilot push to GitHub before asking Claude to review — SHA is the ground truth
- Pushing all related file changes in a single commit = atomic, reviewable changesets

---

## Prompt Patterns That Didn't Work

- Broad prompts touching multiple sections miss consistency issues
- Vague delete prompts don't work — name files explicitly
- Azure infrastructure prompts without security reminders write connection strings to config
- Documentation generation prompts can produce files with bad security guidance — always review
- Prompts that assume a repository pattern when the service uses DbContext directly — verify architecture first
- Adding a field in one prompt doesn't guarantee it cascades to all controller actions — always verify manually
- Asking Copilot to verify small changes it hasn't made yet — it will hallucinate completion

---

_Last Updated: April 2026_
