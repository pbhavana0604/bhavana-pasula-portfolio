# Mapping to the Iowa DOM DoIT posting — requirement by requirement

Client: State of Iowa, Department of Management, Division of
Information Technology (DOM DoIT). Role: Senior .NET Developer /
Front-End UI/UX Developer. Role weighting: .NET/C# ~40%,
front-end/UI/UX ~40%, middleware/APIs ~10%, SQL Server ~10%,
accessibility as a heavy core requirement across all of it.

| Posting requirement | Where to see it in this repo | Notes |
|---|---|---|
| .NET and C# development (40%) | `src/IowaBenefitsPortal/` — controllers, services, repositories, DI, validation, middleware, logging | ASP.NET Core 8, clean layers, SOLID/repository patterns |
| Front-end HTML5, CSS, Sass, JavaScript (40%) | `wwwroot/*.html`, `wwwroot/css/main.css`, `wwwroot/js/*.js`, `frontend/scss/*.scss` | Semantic HTML, Sass tokens/maps/mixins, plain JS with no build step; revised resume now explicitly lists Sass/SCSS, UI/UX implementation and usability principles |
| UX/UI, design thinking, responsive design | `wwwroot/apply.html` (full journey: entry → validation → error recovery → confirmation → status), `wwwroot/design-system.html`, responsive CSS at 320px–desktop | One complete user journey, not isolated widgets |
| Enterprise design system (not just Bootstrap/Material) | `docs/DESIGN_SYSTEM.md`, `frontend/scss/`, `design-system.html` | Explains design system vs. component library vs. CSS framework; adoption without rewriting pages. Revised resume: enterprise design systems and design tokens are listed skills, and at UCLA Health the author used reusable Angular Material components, design tokens, standardized layouts and consistent navigation |
| WCAG 2.1 AA / Section 508, axe, JAWS | `docs/ACCESSIBILITY.md`, accessible pages, HTML smoke tests | Patterns + checklists + before/after fixes, including the remediation types on the revised resume: missing labels, keyboard interactions, focus order and dynamic-content accessibility; **no compliance/certification claimed** |
| MVC and Web Forms where they remain | `docs/LEGACY_MODERNIZATION.md`, `code-samples/legacy/` | Web Forms sample + modern equivalent; marked not compiled in .NET 8 |
| Middleware and APIs (10%) | `Middleware/`, versioned `api/v1` controllers, `/health` | Exception handling, security headers, request logging |
| SQL Server (10%) | `database/sql/*.sql`, `docs/SQL_SERVER.md`, SQL repository | Schema, constraints, parameterized queries, stored procedure |
| Azure DevOps and CI/CD | `azure-pipelines.yml`, `.github/workflows/ci.yml`, `docs/CI_CD.md` | Restore → build → test → publish |
| Secure coding, encryption, sensitive data | `docs/SECURITY_AND_CONFIG.md`, middleware, parameterized SQL, no-PII logging | No secrets in repo; encryption guidance for production |
| Government / healthcare experience | Resume (State of Maine eligibility, UCLA Health) — **not** this demo | This demo is fictional; see "Honest gaps" |
| Angular | Resume (Angular, TypeScript, RxJS, NgRx, Angular Material) | **Not** a demo Angular app — deliberately, to avoid a token app. Revised resume adds UCLA Health use of reusable Angular Material components and design tokens |
| GitHub | `docs/PUBLISHING.md`, `.github/workflows/ci.yml` | Repository-ready; publishing is a manual step for the author |
| AWS | `docs/CI_CD.md` future path only | CodePipeline/CodeBuild/ECS sketch; no cloud resources created |
| Iowa location preference | Revised resume header (not demo code) | Author is based in Waukee, Iowa, and is willing to relocate. No Iowa employment is claimed; this addresses location preference only |

## Honest gaps — stated plainly, not papered over

- **No State of Iowa employment or work is claimed.** The author
  has not worked for Iowa; her government experience is the
  State of Maine (Dec 2021–Jun 2023).
- **No accessibility certification and no formal WCAG training
  are claimed.** The resume records Section 508 / WCAG 2.1 work
  and axe-core use, not a DHS Trusted Tester, IAAP WAS, CPACC or
  equivalent credential. The posting lists certification/training
  as required/preferred; this portfolio demonstrates hands-on
  accessible code and testing workflow, but it does not close
  that credential gap, and a reviewer should weigh it as such.
- **No degree is stated in this portfolio.** The resume file
  supplied for this portfolio does not list a degree; none is
  invented here. The posting asks for a bachelor's in HCI/design/
  CS or equivalent experience — the author should confirm her
  education separately on the resume itself.
- **No production deployment, no government authorization.**
  This is a local demo. Azure/AWS deployment steps are manual
  options in `docs/PUBLISHING.md`, not completed deployments.
- **JAWS manual testing, a live SQL Server run, and an axe
  browser run were not performed in this build environment**
  (see README "Not verified"). The code is written to be testable
  by those tools; the runs themselves remain to be done.
- **Revised resume (2026-10-27):** the portfolio was updated to
  match `Bhavana_.NET_Developer_Resume.docx`. That resume also
  lists no degree and no accessibility certification, so neither
  is claimed here. Its header lists Waukee, Iowa, willingness to
  relocate, and a LinkedIn profile; only city/state, relocation
  willingness and the LinkedIn link are reflected in this repo —
  street address, phone and email are deliberately not published
  here.
