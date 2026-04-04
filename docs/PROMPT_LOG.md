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
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Full CRUD with 7 filter params and pagination ✅
  - ISportsCardService + PagedResult<T> in Core/Interfaces ✅
  - SportsCardService with smart IsAvailable default filter ✅
  - Three DTOs + PagedSportsCardResponse in API/Models ✅
  - ProducesResponseType on all endpoints, CreatedAtAction on POST ✅
  - Entity → DTO mapping (never returns raw entities) ✅
  - ILogger + exception handling throughout ✅
  - Minor: redundant timestamp set in controller (AppDbContext already handles this)
  - Minor: PagedResult<T> in ISportsCardService.cs — should be its own file
  - Flag: AZURE_DEPLOYMENT.md Step 2 instructed appsettings.json — fixed in follow-up

### Prompt 3.2 — Unit Tests
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
Create xUnit unit tests for SportsCardsController in the
SportsCardStore.UnitTests project. Mock ISportsCardService
using Moq. Cover these scenarios:

1. GetAllCards returns 200 OK with paged results when service returns data
2. GetAllCards returns 200 OK with empty results when no cards exist
3. GetCard returns 200 OK with card data when card exists
4. GetCard returns 404 NotFound when card does not exist
5. CreateCard returns 201 Created with location header when model is valid
6. CreateCard returns 400 BadRequest when model state is invalid
7. UpdateCard returns 200 OK when card exists and model is valid
8. UpdateCard returns 404 NotFound when card does not exist
9. DeleteCard returns 204 NoContent when card exists
10. DeleteCard returns 404 NotFound when card does not exist

Use FluentAssertions for assertions. Follow Arrange/Act/Assert pattern.
Add XML doc comments explaining what each test validates.
Add a reference to SportsCardStore.Infrastructure in the test project
csproj if needed.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - **12 tests generated** — went beyond the 10 requested by adding exception handling test and UpdateCard validation test ✅
  - `Controllers/` subfolder created in test project — correct organization ✅
  - All four src projects referenced in csproj: Core, Infrastructure, API, Shared ✅ — API reference critical for DTO access
  - Test naming convention: `MethodName_Condition_ExpectedResult` — professional standard ✅
  - `It.IsAny<>()` matchers used correctly for all 10 service method parameters ✅
  - `Verify()` calls on Delete tests confirm service was called exactly once or never ✅
  - `SerializableError` type check on BadRequest responses ✅
  - `CreatedAtActionResult.ActionName` and `RouteValues` checked on POST ✅
  - Status codes AND response body values validated on every test ✅
  - Real player names used in test data (Trout, Judge, Acuña, Ohtani, Betts) ✅
  - **Note:** Original prompt said "mock the repository" but SportsCardService uses AppDbContext directly — prompt was correctly updated to mock ISportsCardService at controller level instead. This is the right testing approach.

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
- **Prompt:**
```
Create a C# class called CardListingAgent that accepts a raw card
description string, calls the Anthropic Claude API using HttpClient,
and returns a structured CardListing object with Title, Description,
SuggestedPrice, Tags, and Category. Use System.Text.Json for
deserialization. Include error handling and logging via ILogger.
```
- **Output Rating:** ⬜ Pending

### Prompt 8.2 — Inventory Import Agent
- **Tool:** *(to be determined)*
- **Output Rating:** ⬜ Pending

### Prompt 8.3 — Price Research Agent
- **Tool:** *(to be determined)*
- **Output Rating:** ⬜ Pending

---

## CI/CD Pipeline

### Prompt CI.1 — Azure Pipelines YAML Pipeline
- **Tool:** GitHub Copilot Chat
- **Output Rating:** ⬜ Pending

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
| 15 | Explicit security instructions in Azure prompts were respected — lesson carried forward successfully | Phase 2 |
| 16 | Documentation files can also contain bad security guidance — review all generated docs, not just code | Phase 3 |
| 17 | When service uses AppDbContext directly (no repository pattern), mock the interface one level up (ISportsCardService) rather than trying to mock the DB context — produces cleaner, faster unit tests | Phase 3 |
| 18 | Copilot added 2 bonus tests beyond what was requested (exception handling + validation) — it correctly infers missing test scenarios from the existing controller code | Phase 3 |

---

## Prompt Patterns That Worked Well

- Numbered list of specific changes = clean targeted edits
- Technology + output shape + learning goal in agent prompts = detailed documentation
- Explicit file names when deleting = reliable results
- Entity prompts with field types + validation rules + target project = production-quality output
- Fluent API + separate configuration class in DbContext prompts = well-structured EF config
- Real player names + grading company mix in seed prompts = accurate domain data
- Both `add` and `update` migration commands in one prompt = complete runnable workflow
- Explicit security instructions in Azure prompts ("Do NOT write credentials to any file") = no credential exposure
- **Mocking at the interface level (ISportsCardService) rather than the implementation (AppDbContext) = cleaner, faster, more maintainable tests**

---

## Prompt Patterns That Didn't Work

- Broad prompts touching multiple sections miss consistency issues
- Vague delete prompts don't work — name files explicitly
- Azure infrastructure prompts without security reminders write connection strings to config
- Documentation generation prompts can produce files with bad security guidance — always review
- **Prompts that assume a repository pattern exists when the service uses DbContext directly — always verify the actual architecture before writing the test prompt**

---

*Last Updated: April 2026*
