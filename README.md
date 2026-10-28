# Bhavana Pasula — Senior .NET Full Stack Developer | Portfolio & Code Samples

**General portfolio, not built for one job posting.** By Bhavana Pasula, Senior .NET Full Stack Developer — based in Waukee, Iowa, willing to relocate.

Professional profile: [LinkedIn — Bhavana Pasula](https://www.linkedin.com/in/bhavana2708/)

> **Fictional sample data only.** This is not a real client system. No real patient, citizen, customer or passenger data, no credentials, and no client authorisation are involved. Street address, phone and email are intentionally not published in this repo; use the contact information on the submitted resume.

## What is in here

A runnable ASP.NET Core 8 demo, an accessible front-end design system, and code samples mapped to 10 years of resume experience:

- **Healthcare** — UCLA Health, Jul 2025–present: Angular dashboards, SignalR, FHIR R4 / HL7 v2, Azure OpenAI / RAG
- **Banking** — Fifth Third Bank, Jul 2023–Jun 2025: loan/payment microservices, React, Kafka/MSK, Bedrock/RAG
- **Government** — State of Maine, Dec 2021–Jun 2023: VB.NET/Web Forms → ASP.NET Core, citizen/caseworker portals, Section 508 / WCAG 2.1
- **Airline** — Southwest Airlines, Dec 2018–Nov 2021: reservation/check-in microservices, Angular, Azure Service Bus
- **IT services** — Hexaware Technologies, Apr 2017–Aug 2018: support/ticket modules, ASP.NET MVC/Web API, React, AWS

## Quick start — run the demo

Prerequisites: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (8.0.x). No database, no Node, and no Sass build are required.

```bash
git clone https://github.com/pbhavana0604/bhavana-pasula-portfolio.git
cd bhavana-pasula-portfolio
dotnet restore IowaBenefitsPortal.sln
dotnet run --project src/IowaBenefitsPortal/IowaBenefitsPortal.csproj --urls http://localhost:5111
```

Then open <http://localhost:5111/> — the portfolio landing page.

Try the live demo: **Benefits demo** → **Apply (demo)** → submit with fictional details → copy the reference (e.g. `IA-DEMO-2026-000201`) → **Check status**. Seeded demo records also work: `IA-DEMO-2026-000101`, `IA-DEMO-2026-000102`.

### Tests

```bash
dotnet test IowaBenefitsPortal.sln
```

### API quick check (with the app running)

```bash
curl http://localhost:5111/health
curl http://localhost:5111/api/services
curl -X POST http://localhost:5111/api/applications \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Alex","lastName":"Example","city":"Des Moines","zipCode":"50309","householdSize":3,"monthlyIncome":1500,"serviceIds":["food-assistance"],"consent":true}'
curl http://localhost:5111/api/applications/IA-DEMO-2026-000101
```

Full contract: `docs/ARCHITECTURE.md`. Versioned routes (`/api/v1/...`) work identically to the unversioned aliases.

## How the frontend was built

General portfolio best practice, applied here: hero with role + location + relocation, runnable demo above the fold, project grid, experience timeline, skills list — no stock photos, no fake testimonials, no invented metrics.

Technically: semantic HTML5, one `h1` per page, skip link, landmarks, real `<label>` elements, error summaries linked to fields, `aria-live` status regions, visible focus, and Sass/SCSS design tokens. The compiled CSS in `wwwroot/css/main.css` is checked in, so the site runs with no Sass build step. Source SCSS is in `frontend/scss/` (`_tokens`, `_mixins`, `_base`, `_components`, `main.scss`).

Pages (served by the app at `/`):

- `index.html` — portfolio home
- `projects.html` — all projects, with how the frontend was built
- `code-samples.html` — code samples index
- `benefits.html` — the runnable benefits portal demo (one sample project inside the general portfolio)
- `apply.html` / `status.html` / `services.html` — the demo journey
- `design-system.html` — live components and tokens

## Directory structure

```
bhavana-pasula-portfolio/
├── README.md, azure-pipelines.yml, .github/workflows/ci.yml, LICENSE, .gitignore
├── src/IowaBenefitsPortal/          # ASP.NET Core 8 runnable demo app
│   ├── Program.cs                   # DI, middleware order, static files, JSON options
│   ├── Controllers/  Dtos/  Models/  Services/  Repositories/  Middleware/
│   └── wwwroot/                     # portfolio home, projects, code samples, demo pages
│       ├── css/main.css             # compiled design-system CSS (checked in, no build needed)
│       └── js/  app.js  apply.js  status.js  services.js
├── tests/IowaBenefitsPortal.Tests/  # xUnit: eligibility rules, API 201/400/404, HTML smoke tests
├── frontend/scss/                   # _tokens, _mixins, _base, _components, main.scss
├── database/sql/                    # 01_schema, 02_seed_fictional, 03_queries (stored procedure)
├── code-samples/
│   ├── csharp/                      # HealthcareDashboard, LoanApi, FlightReservation, SupportTicket
│   └── legacy/                      # Web Forms sample + modern ASP.NET Core equivalent (not compiled)
└── docs/                            # ARCHITECTURE, DESIGN_SYSTEM, ACCESSIBILITY, SQL_SERVER,
                                     # LEGACY_MODERNIZATION, CI_CD, SECURITY_AND_CONFIG,
                                     # PORTFOLIO, PUBLISHING
```

Note on naming: the runnable app folder is still called `IowaBenefitsPortal` because that demo was the first project built. Renaming its namespace would break its tests for no reviewer benefit, so the folder name is kept and the repo, README and landing page are general. The benefits demo is one project inside this general portfolio, not the portfolio itself.

## Accessibility — honest limits

Every page uses landmarks, one `h1`, logical heading order, a skip link, visible focus, real labels, error summaries linked to fields, and an `aria-live` status region. Remediation focus matches the resume: missing labels, keyboard interaction, focus order, and dynamic content accessibility.

**No certification is claimed, and no page is claimed to be fully WCAG compliant or Section 508 compliant.** Automated checks are a regression net, not a compliance claim. Manual keyboard and screen-reader testing are still required — see `docs/ACCESSIBILITY.md` for the full checklist, axe and JAWS workflow.

## Deployment

- **Full app (API + pages):** Azure App Service, or any host that runs .NET 8 — see `docs/PUBLISHING.md`. GitHub Pages alone serves only static pages; the API needs the running app.
- **Static preview only:** GitHub Pages can serve `wwwroot/` as a preview of the pages, with API calls failing until the app is running — that limitation is stated on the preview, not hidden.

## Configuration and security

No secrets are in this repository. The connection string is empty by default and the app runs on the in-memory store; to use SQL Server, set `ConnectionStrings__IowaBenefits` via environment variable / Key Vault / Secrets Manager. Details: `docs/SECURITY_AND_CONFIG.md` and `docs/SQL_SERVER.md`.

## Not verified in this build environment

Stated honestly, so a reviewer is not misled:

- **JAWS manual testing** — no JAWS licence in this environment.
- **axe browser run** — not performed here; documented as a manual step with CLI instructions.
- **Live SQL Server run** — no SQL Server instance here; the repository code compiles and its SQL is parameterized, but the database scripts have not been executed against a live server.
- **Public deployment** — no hosting was provisioned in this build; publishing steps are manual in `docs/PUBLISHING.md`.
- **Sass compilation** — verified in the earlier build: `npx sass frontend/scss/main.scss src/IowaBenefitsPortal/wwwroot/css/main.css` completed with no warnings, and that generated file is the checked-in `wwwroot/css/main.css`.
