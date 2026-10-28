# Security and configuration

## Rules enforced in this repository

- **No secrets, ever, in source.** No connection strings with
  passwords, no API keys, no tokens. `appsettings.json` leaves
  the connection string empty; CI uses no secrets.
- **Configuration precedence (ASP.NET Core default):**
  `appsettings.json` → `appsettings.{Environment}.json` →
  environment variables → Key Vault / Secrets Manager in
  production. For local secrets, use `dotnet user-secrets`
  (development only, stored outside the repo).
- **Validation on the server, always.** Client validation in
  `apply.js` mirrors the server rules for usability; the server
  re-validates with DataAnnotations plus business rules and never
  trusts the client.
- **Parameterized SQL only.** See `SqlServerApplicationRepository`
  and `database/sql/03_queries.sql`.
- **Error handling that does not leak:** the global exception
  middleware returns a generic problem body and logs the detail
  server-side only.
- **Logging without personal data:** logs carry method, path,
  status, duration and (for applications) the reference number
  and outcome — never names, income, ZIP or request bodies.
- **Security headers** on every response, including static pages
  (`SecurityHeadersMiddleware`): nosniff, frame denial, strict
  referrer policy, and a restrictive content-security policy
  (`default-src 'self'`).
- **HTTPS redirection** outside Development; CORS is off because
  the front end is served by the same app.

## Encryption considerations for a real system (not in this demo)

- TLS in transit (enforced by hosting + HSTS in production).
- SQL Server TDE for data at rest; column-level encryption
  (Always Encrypted) for any genuinely sensitive field — this
  demo collects none, by design.
- Secrets in Azure Key Vault or AWS Secrets Manager with
  managed identities (no stored passwords), key rotation, and
  audit logging of secret access.
- This demo has **no government security authorization** and
  must not be described as authorized, accredited, or approved
  for handling real citizen data.
