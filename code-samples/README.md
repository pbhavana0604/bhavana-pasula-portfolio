# Code samples — index

Start on the portfolio site at **Code samples** (`src/IowaBenefitsPortal/wwwroot/code-samples.html`),
which links to every section below. The runnable code is the demo app itself;
the files in this folder are illustrative patterns with fictional data only.

| Area | Where to look | Runnable or illustrative? |
|---|---|---|
| C# / .NET, layered ASP.NET Core | `src/IowaBenefitsPortal/Controllers/`, `Services/`, `Models/`, `Dtos/`, `Program.cs` | Runnable — `dotnet run` |
| REST APIs and middleware | `src/IowaBenefitsPortal/Middleware/`, `docs/ARCHITECTURE.md` | Runnable |
| SQL Server | `database/sql/01_schema.sql`, `02_seed_fictional.sql`, `03_queries.sql`, `src/IowaBenefitsPortal/Repositories/` | SQL not executed against a live server in this environment; repository compiles, queries are parameterised |
| HTML5 and JavaScript | `src/IowaBenefitsPortal/wwwroot/*.html`, `wwwroot/js/apply.js`, `status.js`, `services.js`, `app.js` | Runnable |
| Sass / SCSS | `frontend/scss/_tokens.scss`, `_mixins.scss`, `_base.scss`, `_components.scss`, `main.scss` | Source; compiled CSS is checked in at `wwwroot/css/main.css` |
| Reusable UI components | `src/IowaBenefitsPortal/wwwroot/design-system.html`, `docs/DESIGN_SYSTEM.md` | Runnable |
| Accessibility remediation | `docs/ACCESSIBILITY.md` (before/after examples, axe + JAWS workflow) | Documentation + HTML smoke tests in `tests/` |
| Automated tests | `tests/IowaBenefitsPortal.Tests/PortalTests.cs` | Runnable with `dotnet test` (see README for observed status) |
| Azure DevOps CI/CD | `azure-pipelines.yml`, `.github/workflows/ci.yml`, `docs/CI_CD.md` | Sample pipelines, not executed against a hosted account here |
| Healthcare pattern (UCLA Health work, fictional data) | `code-samples/csharp/HealthcareDashboard.cs` | Illustrative, not compiled standalone |
| Banking pattern (Fifth Third Bank work, fictional data) | `code-samples/csharp/LoanApi.cs` | Illustrative, not compiled standalone |
| Airline pattern (Southwest Airlines work, fictional data) | `code-samples/csharp/FlightReservation.cs` | Illustrative, not compiled standalone |
| Support pattern (Hexaware work, fictional data) | `code-samples/csharp/SupportTicket.cs` | Illustrative, not compiled standalone |
| Legacy Web Forms + modern equivalent | `code-samples/legacy/`, `docs/LEGACY_MODERNIZATION.md` | Illustrative, not compiled in .NET 8 |

No real patient, citizen, customer, or passenger data is used anywhere.
No accessibility certification, degree, production
deployment, or government security authorisation is claimed.
