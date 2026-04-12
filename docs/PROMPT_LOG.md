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

### Prompt 4.1 — React + Vite Frontend Scaffold
- **Tool:** GitHub Copilot Chat + Claude (fixes)
- **Date:** April 2026
- **Output Rating:** ⚠️ Needed Tweaking
- **Notes:**
  - Created `src/SportsCardStore.Web/` with Vite + React 18 + TypeScript + Tailwind + react-router-dom v6 ✅
  - Correct folder structure: `components/`, `pages/`, `services/`, `types/`, `utils/` ✅
  - `VITE_API_BASE_URL` from `.env` with localhost fallback — never hardcoded ✅
  - `.env` and all local env variants correctly gitignored, `.env.example` present ✅
  - TypeScript interfaces match C# DTOs exactly including `isBowmanFirst`, `parallelName`, `printRun` ✅
  - Enum values match C# integer backing values ✅
  - `apiService.ts` uses `fetch` + `URL`/`searchParams` — clean query building ✅
  - Both pages (`CardListPage`, `CardDetailPage`) on correct routes ✅
  - `LoadingSpinner`, `ErrorMessage`, `CardItem`, `CardFilters`, `Pagination` components all created ✅
  - **Fix 1 — `isAutograph` filter missing from API call:** Defined in `CardFilters` type but never appended to URL params. Claude added the missing `searchParams.append` block ✅
  - **Fix 2 — Build artifacts committed:** `tsconfig.app.tsbuildinfo` and `tsconfig.node.tsbuildinfo` were committed to the repo. Claude added `*.tsbuildinfo` to `.gitignore`. Run `git rm --cached src/SportsCardStore.Web/tsconfig.app.tsbuildinfo src/SportsCardStore.Web/tsconfig.node.tsbuildinfo` locally to stop tracking them ✅

**To run locally:**
```
cd src/SportsCardStore.Web
cp .env.example .env
npm install
npm run dev
```

---

## Phase 5 — Stripe Payments

*(Skipped for now — self-contained phase, does not block Phase 6 or 7)*

---

## Phase 6 — Image Upload Pipeline

### Prompt 6.1 — Image Upload API Endpoints
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - `POST /api/sportscards/{id}/image` — uploads image to Azure Blob Storage, updates `ImageUrl` on card ✅
  - `DELETE /api/sportscards/{id}/image` — deletes blob and clears `ImageUrl` on card ✅
  - `IBlobStorageService` (from Phase 2) injected alongside `ISportsCardService` in controller ✅
  - File size validated server-side (10MB hard limit) ✅
  - Extension whitelist: `.jpg`, `.jpeg`, `.png`, `.gif`, `.bmp`, `.webp`, `.tiff` ✅
  - Old image auto-deleted from Blob Storage before uploading replacement — no orphaned blobs ✅
  - `ExtractFileNameFromUrl()` private helper safely parses blob URL back to filename for deletion ✅
  - `UpdateImageUrlAsync(id, url)` added to `ISportsCardService` and `SportsCardService` ✅
  - ProducesResponseType annotations on both new endpoints ✅

### Prompt 6.2 — ImageUpload Frontend Component
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - `ImageUpload.tsx` component with `cardId`, `hasImage`, `onUploadSuccess`, `onError` props ✅
  - Hidden `<input type="file">` triggered by visible button — no browser default file input UI ✅
  - Client-side validation mirrors server: JPG/PNG only, 10MB max ✅
  - Button label is context-aware: "Upload Image" (no image) vs "Replace Image" (existing) ✅
  - Inline upload spinner with `animate-spin` during upload ✅
  - File input reset after every upload attempt so same file can be re-selected ✅
  - `apiService.uploadCardImage(cardId, file)` method added as `FormData` multipart POST ✅
  - Component integrated into `CardDetailPage` ✅

---

## Phase 7 — Admin Panel

*(Prompts to be added as this phase begins)*

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
  - DTOs correctly moved to Shared project unprompted — strong architectural decision ✅

### Prompt 8.2.1 — Add Entity Fields + Migration
- **Output Rating:** ✅ Great — SetName, IsRookie, IsAutograph, IsRelic added. Migration `20260409113542_AddInventoryFields` applied ✅

### Prompt 8.2.2 — Add IsBowmanFirst Field
- **Output Rating:** ✅ Great — Added to entity, DTOs, service filter, controller param. Migration `20260409114438_AddIsBowmanFirstField` applied ✅

### Prompt 8.2.3 — Fix InventoryImportAgent IsBowmanFirst Mapping
- **Tool:** Claude (direct GitHub push)
- **Output Rating:** ✅ Great — Copilot hallucinated completion twice (SHA unchanged). Claude pushed fix directly ✅

### Prompt 8.2.4 — Fix Controller CreateCard/UpdateCard Missing Field Mappings
- **Tool:** Claude (direct GitHub push)
- **Output Rating:** ✅ Great — All new fields mapped. Claude added `MapToResponse()` helper for consistent responses ✅

### Prompt 8.2.5 — Add Parallel Fields (ParallelName + PrintRun)
- **Tool:** Claude (direct GitHub push)
- **Output Rating:** ✅ Great — Added across all 6 layers in one commit. Migration `20260409204603_AddParallelFields` applied ✅

### Prompt 8.2.6 — Default Condition to "Near Mint or Better"
- **Tool:** Claude (direct GitHub push)
- **Output Rating:** ✅ Great — `DefaultCondition` constant added to InventoryImportAgent. Also applied by Claude when parsing voice-dictated card data ✅

---

## Current Excel Column Schema (21 columns)

| # | Column | Required | Default |
|---|---|---|---|
| 1 | Player Name | ✅ | — |
| 2 | Team | ✅ | — |
| 3 | Brand | ✅ | — |
| 4 | Set Name | ✅ | — |
| 5 | Card Number | ✅ | — |
| 6 | Year | ✅ | — |
| 7 | Sport | ✅ | — |
| 8 | Rookie | | false |
| 9 | Autograph | | false |
| 10 | Relic | | false |
| 11 | Bowman First | | false |
| 12 | Grading Company | ✅ | — |
| 13 | Card Grade | | null (blank for Raw) |
| 14 | Condition | | Near Mint or Better |
| 15 | Price | ✅ | — |
| 16 | Quantity | ✅ | — |
| 17 | Parallel | | null (blank for base) |
| 18 | Print Run | | null (blank for non-numbered) |
| 19 | Description | | null |
| 20 | Image Url | | null |
| 21 | Is Available | | true |

---

### Prompt 8.1 — Card Listing Agent
- **Tool:** GitHub Copilot Chat + Claude (fixes)
- **Date:** April 2026
- **Output Rating:** ⚠️ Needed Tweaking
- **Notes:**
  - Created `src/CardListingAgent/` with correct Anthropic API headers, env var key handling, response validation ✅
  - **Fix 1 — `CardListing` model missing:** Copilot referenced a return type that didn't exist. Claude added it to Shared ✅
  - **Fix 2 — Outdated model:** `claude-3-sonnet-20240229` → `claude-sonnet-4-6` as a named constant ✅
  - **Fix 3 — Broken DI wiring:** Typed HttpClient DI pattern doesn't resolve via `GetRequiredService<HttpClient>()`. Simplified to `new HttpClient()` ✅
  - **Duplicate class resolution:** Copilot later added `CardListing` to `SportsCardDtos.cs` causing a build failure. Copilot correctly emptied the stub file — `CardListing` now lives in `SportsCardDtos.cs` ✅

**To run:**
```
set ANTHROPIC_API_KEY=your-key-here
dotnet run --project src/CardListingAgent "2025 Bowman Draft Dean Curley BDC-129 Cleveland Guardians Raw Near Mint Bowman First"
```

---

### Prompt 8.3 — Price Research Agent
- **Tool:** GitHub Copilot Chat + Copilot (fixes)
- **Date:** April 2026
- **Output Rating:** ⚠️ Needed Tweaking
- **Notes:**
  - `IPricingSource` interface in `Interfaces/`, `EbayPricingSource` in `Sources/`, orchestrator in `PriceResearchAgent.cs` ✅
  - `PricingModels.cs` added to Shared — `CardPricingRequest`, `PricingResult`, `RecentSale`, `PriceResearchResponse` ✅
  - Confidence: 10+ sales = High, 5+ = Medium, 1+ = Low. Listing price = average × 1.05/1.10 clamped ✅
  - **Fix 1 — Wrong eBay API:** Browse API (active listings) → Finding API with `findCompletedItems` + `SoldItemsOnly=true` ✅
  - **Fix 2 — Keyword filter too broad:** `"auto"` and `"jersey"` blocked autograph/relic cards — replaced with `"lot of"`, `"complete set"`, `"break"`, `"case"` ✅
- **Next steps:** Card Ladder API — contact [email protected] re: Pro subscription API access

**To run:**
```
set EBAY_APP_ID=your-ebay-app-id
dotnet run --project src/PriceResearchAgent -- "Mike Trout" 2023 "Topps" "Chrome" "PSA" "9.5"
```

---

## CI/CD Pipeline

### Prompt CI.1 — Azure Pipelines YAML Pipeline
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - `trigger: main`, `ubuntu-latest`, `.NET 10.x` SDK ✅
  - `restore` → `build --no-restore` → `test --no-build` — no redundant work ✅
  - Code coverage via `XPlat Code Coverage` added unprompted ✅
  - Deploy stage: `dependsOn: Build`, branch condition, `environment: production`, `runOnce` strategy ✅
  - `$(AzureServiceConnection)` — variable reference, never hardcoded ✅
  - No credentials anywhere in the file ✅

---

## Lessons Learned

| # | Lesson | Phase |
|---|---|---|
| 1 | Copilot defaults to enterprise-scale answers — always review cost and complexity assumptions | Phase 0 |
| 2 | Copilot makes smart architectural decisions unprompted — don't always override | Phase 0 |
| 3 | A single prompt fix doesn't cascade — check related sections after targeted changes | Phase 0.2 |
| 4 | Name files explicitly when asking Copilot to delete them | Phase 0.3 |
| 5 | Copilot proactively adds packages when upgrading frameworks | Phase 0.3 |
| 6 | Copilot creates correct subfolder structure unprompted when project signals clean architecture | Phase 1 |
| 7 | Verify PK type consistency early — int vs Guid cascades through many files | Phase 1 |
| 8 | Copilot adds timestamp auto-update in SaveChanges unprompted when entities have date fields | Phase 1 |
| 9 | Copilot exceeds index scope — review generated indexes, extras have storage/write costs | Phase 1 |
| 10 | EnsureCreatedAsync() bypasses migrations — always use MigrateAsync() with a real database | Phase 1/2 |
| 11 | **Critical:** Azure setup writes credentials into config files — review appsettings.json before every merge | Phase 2 |
| 12 | Exposed credentials must be rotated immediately — bots scan public repos continuously | Phase 2 |
| 13 | Use dotnet user-secrets locally, Azure App Service Configuration for production | Phase 2 |
| 14 | Add credential file patterns to .gitignore before creating those files | Phase 2 |
| 15 | Explicit security instructions in Azure/CI prompts prevent credential exposure — lesson held from Phase 2 through CI.1 | Phase 2 / CI |
| 16 | Documentation files can also contain bad security guidance — review all generated docs | Phase 3 |
| 17 | Mock at the interface level (ISportsCardService) not the DB context — cleaner unit tests | Phase 3 |
| 18 | Copilot adds bonus output beyond what was requested — infers missing scenarios from existing code | Phase 3 |
| 19 | When building a standalone agent that needs shared DTOs, Copilot correctly moves them to Shared — keep these unprompted architectural improvements | Phase 8 |
| 20 | Adding a new field to the entity doesn't automatically cascade to controller action mappings — always verify CreateCard/UpdateCard manually | Phase 8 |
| 21 | **Always verify by SHA, not by Copilot's confirmation** — if SHA hasn't changed, nothing happened | Phase 8 |
| 22 | A single `MapToResponse()` helper in the controller ensures all fields returned consistently | Phase 8 |
| 23 | Domain knowledge drives better data models — hobby expertise (parallels, print runs, Bowman Firsts) produced richer schema than any generic template | Phase 8 |
| 24 | Voice dictation workflow (Claude Desktop mic → Claude parses → Excel row) eliminates manual data entry — design defaults around what's most commonly true | Phase 8 |
| 25 | Always verify the output type referenced by a new agent actually exists in the solution — Copilot generated a `CardListing` return type that wasn't defined anywhere | Phase 8 |
| 26 | AI model strings go stale — use a named constant so updates require only one change | Phase 8 |
| 27 | Verify which API endpoint an agent is using — eBay Browse API (active listings) vs Finding API (sold prices) is a critical distinction for pricing data | Phase 8 |
| 28 | Review keyword exclusion filters with domain knowledge — "auto" and "jersey" are legitimate card terms, not exclusion candidates | Phase 8 |
| 29 | CI/CD YAML generated correctly first attempt when security instructions were explicit — `--no-restore` and `--no-build` flags used correctly, code coverage added unprompted | CI |
| 30 | TypeScript frontend: a filter defined in a type interface is not automatically wired to the API call — verify every filter param is actually appended to the URL | Phase 4 |
| 31 | Add `*.tsbuildinfo` to `.gitignore` for any TypeScript/Vite project — these build cache files are generated locally and should never be committed | Phase 4 |
| 32 | Mirror server-side validation on the client — `ImageUpload.tsx` and the API controller enforce the same file type and size limits, preventing wasted round trips | Phase 6 |
| 33 | Delete the old blob before uploading a replacement — without this, every image update leaves an orphaned file in Azure Blob Storage that accrues storage cost | Phase 6 |
| 34 | Hide the native file input and trigger it from a custom button — gives full control over the UI state (spinner, label changes) without the inconsistent browser default | Phase 6 |

---

## Prompt Patterns That Worked Well

- Numbered list of specific changes = clean targeted edits
- Technology + output shape + learning goal in agent prompts = detailed documentation
- Explicit file names when deleting = reliable results
- Entity prompts with field types + validation rules + target project = production-quality output
- Fluent API + separate configuration class in DbContext prompts = well-structured EF config
- Real player names + grading company mix in seed prompts = accurate domain data
- Both `add` and `update` migration commands in one prompt = complete runnable workflow
- Explicit security instructions in Azure and CI prompts = no credential exposure, every time
- Mocking at the interface level (ISportsCardService) = cleaner, faster, more maintainable tests
- Documenting the Excel column schema before prompting the import agent = precise output
- Always have Copilot push to GitHub before asking Claude to review — SHA is the ground truth
- Pushing all related file changes in a single commit = atomic, reviewable changesets
- Designing a pricing agent with an `IPricingSource` interface = swap data sources without rewriting logic
- Reviewing fixes before updating the prompt log = log reflects verified state, not claimed state
- Specifying both client-side and server-side validation requirements in file upload prompts = consistent behavior across the stack

---

## Prompt Patterns That Didn't Work

- Broad prompts touching multiple sections miss consistency issues
- Vague delete prompts don't work — name files explicitly
- Azure infrastructure prompts without security reminders write connection strings to config
- Documentation generation prompts can produce files with bad security guidance — always review
- Prompts that assume a repository pattern when the service uses DbContext directly — verify first
- Adding a field in one prompt doesn't guarantee it cascades to all controller actions — always verify
- Asking Copilot to verify small changes it hasn't made yet — it will hallucinate completion
- Agent prompts that don't specify where the return type model should live — Copilot may reference a class it never creates
- Accepting an agent's API integration without verifying it targets the right endpoint — check active vs sold, v1 vs v2
- Frontend filter types don't automatically wire to API calls — always verify every filter param is appended

---

*Last Updated: April 2026*
