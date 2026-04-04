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
  - Smartly split `Card` and `CardListing` into separate entities without being asked
  - Added `Brand` and `Category` as normalized entities unprompted
  - Added `MediatR`, `FluentValidation`, `AutoMapper` to tech stack unprompted
  - Added `SportsCardStore.Shared` as a fifth project — kept it
  - **Required corrections (see Phase 0.2):** SQL tier too expensive, GitHub Actions vs Azure Pipelines, AD B2C not phased, QnA Maker deprecated, missing personal infrastructure, generic agents

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
2. In DevOps & Deployment, replace "GitHub Actions" with "Azure Pipelines".
3. Move Docker to a "Future Consideration" note.
Keep all other Azure services as-is.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:** All three changes applied cleanly

---

### Prompt 0.2.2 — Fix Authentication Approach
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Prompt:**
```
In PROJECT_PLAN.md Tech Stack Decisions under Authentication & Authorization,
add a note that Azure AD B2C is a future enhancement. Phase 1 uses
ASP.NET Core Identity with JWT Bearer Tokens only.
```
- **Output Rating:** ✅ Great
- **Notes / What Was Changed:**
  - Tech Stack split into Phase 1 and Future Enhancements
  - Azure Services section still listed AD B2C without qualifier — fixed in Prompt 0.2.7

---

### Prompt 0.2.3 — Fix Deprecated QnA Maker Reference
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** Replaced with Azure AI Language + Claude/OpenAI alternative

---

### Prompt 0.2.4 — Add Personal Infrastructure Section
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** Excel inventory, Epson scanner, WhatNot business all added after Project Overview

---

### Prompt 0.2.5 — Replace Generic AI Agents with Specific Hands-On Builds
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** Three specific agents (Card Listing, Inventory Import, Price Research) with technology/output/learning goals documented

---

### Prompt 0.2.6 — Add Azure MCP to Development Workflow
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** AI-Assisted Development Tools section added with Copilot, Azure MCP, Playwright MCP, Claude, Prompt Log

---

### Prompt 0.2.7 — Fix AD B2C in Azure Services Section
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** Fixed consistency gap between Tech Stack and Azure Services sections

---

## Phase 0.3 — Solution Cleanup & .NET Upgrade

### Prompt 0.3.1 — Remove WeatherForecast Placeholder Files
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ⚠️ Needed Tweaking
- **Notes:** First attempt failed — files remained. Required explicit follow-up (0.3.2)

### Prompt 0.3.2 — Remove WeatherForecast Files (Follow-up)
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** Named files explicitly — both removed cleanly. Controllers folder auto-deleted when empty.

### Prompt 0.3.3 — Remove Placeholder Class1.cs Files
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** All placeholder files removed, extra SportsCardStore.Tests project also removed

### Prompt 0.3.4 — Upgrade Solution to .NET 10
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - All projects updated to net10.0, Swashbuckle to 7.0.0
  - Copilot proactively added EF Core SqlServer 10.0.0 to Infrastructure
  - xUnit, Moq, FluentAssertions, coverlet wired into UnitTests
  - .NET 10 = current LTS (released Nov 2025) — correct for new project April 2026

---

## Phase 1 — Solution Scaffolding & Data Model

### Prompt 1.1 — Solution Structure
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** All five src projects + three test projects created with correct references

### Prompt 1.2 — SportsCard Entity Model
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Created `Entities/` and `Enums/` subfolders unprompted
  - XML doc comments on every property
  - Grade nullable — raw cards don't have grades
  - Correct decimal precision, [Url] validation, year range 1800-2100
  - **Note:** int Id used — project plan specifies Guid. Watch for this in DbContext

### Prompt 1.3 — DbContext & EF Configuration
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Auto-updates timestamps in SaveChanges/SaveChangesAsync unprompted
  - Prevents CreatedDate modification on update
  - Six indexes (vs two requested) — PlayerName, Year, composite, Sport, IsAvailable, CreatedDate
  - Full Fluent API with explicit SQL types
  - Minor: duplicate ApplyConfiguration + ApplyConfigurationsFromAssembly calls

### Prompt 1.4 — Seed Data
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Idempotent guard, Seeders/ subfolder, 10 real players across all sports
  - All grading companies represented, Raw cards have Grade = null
  - Program.cs wired with DI, dev-only block, exception handling

---

## Phase 2 — Azure Infrastructure Setup

### Prompt 2.1 — Azure Resource Group & Core Services
- **Tool:** Azure MCP Server (Copilot Agent Mode)
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Resource Group East US, SQL Server Central US (capacity fallback)
  - Basic tier SQL (~$5/mo), B1 App Service (~$13/mo)
  - **Security issue:** Password written into appsettings.json — required immediate remediation

### Phase 2 Security Remediation
- **Date:** April 2026
- Password rotated, both files sanitized by Claude via GitHub MCP
- .gitignore updated with credential file patterns including azure-credentials.json

### Prompt 2.3 — User Secrets Setup
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** User secrets initialized, connection string stored locally, never in repo

### Prompt 2.4 — MigrateAsync + InitialCreate Migration
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - EnsureCreatedAsync replaced with MigrateAsync
  - Migration 20260404180525_InitialCreate — all columns, types, 6 indexes correct
  - Applied to Azure SQL — SportscardDb table live

### Prompt 2.2.1 — Create Azure Blob Storage Account
- **Tool:** Azure MCP Server (Copilot Agent Mode)
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Blob Storage + card-images container created
  - Credentials returned in chat only — security instruction respected ✅

### Prompt 2.2.2 — Add BlobStorageService to Infrastructure
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:**
  - Core/Interfaces/ folder created unprompted
  - Azure.Storage.Blobs 12.27.0 added
  - Unique filename generation (timestamp + Guid), content type detection, fail-fast validation
  - No credentials in generated code ✅

### Prompt 2.2.3 — Register BlobStorageService in Program.cs
- **Tool:** GitHub Copilot Chat
- **Date:** April 2026
- **Output Rating:** ✅ Great
- **Notes:** Registered correctly, appsettings.json stayed clean ✅

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
  - Full CRUD: GET all with 7 filters + pagination, GET by id, POST, PUT, DELETE ✅
  - `ISportsCardService` interface with `PagedResult<T>` in Core/Interfaces ✅
  - `SportsCardService` implementation with IQueryable filter chaining ✅
  - Smart default: filters to `IsAvailable=true` unless explicitly overridden ✅
  - `SportsCardDtos.cs` in API/Models: CreateRequest, UpdateRequest, Response, PagedResponse ✅
  - All DTOs have matching validation annotations ✅
  - `[ProducesResponseType]` on every endpoint, correct status codes (200/201/204/400/404/500) ✅
  - `CreatedAtAction` on POST returning 201 with Location header ✅
  - Controller maps entity → DTO — never returns raw entities to client ✅
  - ILogger + exception handling on every action ✅
  - `ISportsCardService` registered in Program.cs ✅
  - appsettings.json stayed clean ✅
  - **Minor:** Controller manually sets CreatedDate/UpdatedDate on new entities — redundant since AppDbContext.SaveChanges already handles this. Harmless but worth cleaning up when AutoMapper is added
  - **Minor:** `PagedResult<T>` defined inside ISportsCardService.cs — should be its own file at Core/Models/PagedResult.cs
  - **Flag:** New `AZURE_DEPLOYMENT.md` at repo root contains Step 2 instructing users to add connection strings to appsettings.json — contradicts security lessons. Run fix prompt before next session

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

### Prompt 8.2 — Inventory Import Agent
- **Tool:** *(to be determined)*
- **Prompt:** *(to be filled in)*
- **Output Rating:** ⬜ Pending

### Prompt 8.3 — Price Research Agent
- **Tool:** *(to be determined)*
- **Prompt:** *(to be filled in)*
- **Output Rating:** ⬜ Pending

---

## CI/CD Pipeline

### Prompt CI.1 — Azure Pipelines YAML Pipeline
- **Tool:** GitHub Copilot Chat
- **Prompt:** *(to be filled in when ready)*
- **Output Rating:** ⬜ Pending

---

## Lessons Learned

| # | Lesson | Phase |
|---|---|---|
| 1 | Copilot defaults to enterprise-scale answers — always review cost and complexity assumptions | Phase 0 |
| 2 | Copilot made smart architectural decisions unprompted (Card/CardListing split) — don't always override | Phase 0 |
| 3 | A single prompt fix doesn't cascade — check related sections after targeted changes | Phase 0.2 |
| 4 | Name files explicitly when asking Copilot to delete them | Phase 0.3 |
| 5 | Copilot proactively adds packages when upgrading frameworks — it reads context and anticipates needs | Phase 0.3 |
| 6 | Copilot creates correct subfolder structure unprompted when project signals clean architecture | Phase 1 |
| 7 | Verify PK type consistency early — int vs Guid cascades through many files | Phase 1 |
| 8 | Copilot adds timestamp auto-update in SaveChanges unprompted when entities have date fields | Phase 1 |
| 9 | Copilot exceeds index scope — review generated indexes, extras have storage/write costs | Phase 1 |
| 10 | EnsureCreatedAsync() bypasses migrations — always use MigrateAsync() with a real database | Phase 1/2 |
| 11 | **Critical:** Azure setup writes credentials into config files — review appsettings.json before every merge | Phase 2 |
| 12 | Exposed credentials must be rotated immediately — bots scan public repos continuously | Phase 2 |
| 13 | Use dotnet user-secrets locally, Azure App Service Configuration for production | Phase 2 |
| 14 | Add credential file patterns to .gitignore before creating those files | Phase 2 |
| 15 | Explicit "Do NOT write credentials to files" in Azure prompts was respected — lesson carried forward | Phase 2 |
| 16 | Documentation files (like AZURE_DEPLOYMENT.md) can also contain bad security guidance — review all generated docs, not just code files | Phase 3 |

---

## Prompt Patterns That Worked Well

- Numbered list of specific changes = clean targeted edits without touching unrelated content
- Technology + output shape + learning goal in agent prompts = detailed actionable documentation
- Explicit file names when deleting = reliable results
- Field types + validation rules + target project in entity prompts = production-quality output in one shot
- Fluent API + separate configuration class in DbContext prompts = well-structured EF config
- Real player names + grading company mix in seed prompts = accurate domain data
- Both `add` and `update` migration commands in one prompt = complete runnable workflow
- Explicit security instructions in Azure prompts ("Do NOT write credentials to any file") = no credential exposure

---

## Prompt Patterns That Didn't Work

- Broad prompts touching multiple sections miss consistency issues — review related sections after
- Vague delete prompts don't work — name files explicitly with full paths
- Azure infrastructure prompts without security reminders write connection strings to config — always include the instruction
- Documentation generation prompts can produce files with bad security guidance — always review generated docs

---

*Last Updated: April 2026*
