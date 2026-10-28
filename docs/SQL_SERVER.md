# SQL Server — schema, access pattern, production connection

## Files

- `database/sql/01_schema.sql` — `Services`, `Applications`,
  `ApplicationServices`, with `CHECK` constraints (5-digit ZIP,
  household 1–20, income ≥ 0) and an index on submission time.
- `database/sql/02_seed_fictional.sql` — fictional people only
  ("Alex Example", "Jordan Sample").
- `database/sql/03_queries.sql` — parameterized examples, the
  stored procedure `usp_GetApplicationByReference` used by the
  repository, and a reporting view.

## Access pattern (in code)

`Repositories/SqlServerApplicationRepository` uses
`Microsoft.Data.SqlClient` with **parameters for every value** and
a transaction spanning the application row and its service links.
No SQL is ever built by concatenating user input. The repository
is behind `IApplicationRepository`, so the demo's in-memory store
and SQL Server are interchangeable at registration time.

## Production connection (how it would work — not configured here)

1. **Never hardcode** a connection string or password, and never
   commit one. This repo's `appsettings.json` leaves the
   connection string empty on purpose.
2. Supply it by environment variable in production, e.g.
   `ConnectionStrings__IowaBenefits="Server=...;Database=...;Encrypt=true;TrustServerCertificate=false;..."`.
   In Azure, prefer Azure Key Vault referenced from App Service
   configuration; in AWS, AWS Secrets Manager (see `docs/CI_CD.md`
   for the future AWS path). Local development may use
   `dotnet user-secrets` — never a committed file.
3. **Encrypt in transit:** `Encrypt=true` and a valid server
   certificate (`TrustServerCertificate=false`).
4. **Least privilege:** the application's login gets only
   `EXECUTE` on its stored procedures and `INSERT`/`SELECT` on its
   tables — no `db_owner`, no ad-hoc table access from other apps.
5. **Sensitive data:** this demo collects no SSN, date of birth,
   documents or payment data, and a real system would additionally
   need encryption at rest (TDE), column-level encryption for
   sensitive fields, audit logging, and data-retention rules under
   state requirements. None of that is claimed for this demo.

## Running SQL Server locally (optional, not verified here)

```bash
# Docker example only — requires Docker and a SQL Server image.
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=ChangeMe123!" \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
# Then run 01_schema.sql and 02_seed_fictional.sql, and set:
export ConnectionStrings__IowaBenefits="Server=localhost;Database=IowaBenefitsDemo;User Id=sa;Password=ChangeMe123!;Encrypt=false;TrustServerCertificate=true"
```

A live SQL Server run was **not** performed in this environment
(see README "Not verified"); the repository code is compiled and
reviewed, and its SQL is parameterized and transaction-safe.
