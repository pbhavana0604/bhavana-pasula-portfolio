# Modern ASP.NET Core equivalent of the Web Forms sample

The Web Forms page posts back to itself. The modern version separates
concerns instead:

```csharp
// Controller (this repo: Controllers/Controllers.cs)
[HttpGet("{reference}")]
public async Task<IActionResult> Get(string reference, CancellationToken ct)
{
    var application = await applications.GetAsync(reference, ct);
    return application is null
        ? NotFound(new { error = "No application was found for that reference number." })
        : Ok(ApplicationService.ToResponse(application));
}
```

```javascript
// Front end (this repo: wwwroot/js/status.js) — plain fetch, no postback
fetch("/api/applications/" + encodeURIComponent(reference))
  .then(r => r.json())
  .then(render); // renders status as text + icon, never colour alone
```

| Concern | Web Forms | ASP.NET Core (this demo) |
|---|---|---|
| Request | Postback to the same page | `GET /api/applications/{reference}` |
| State | ViewState round-tripped in the page | Stateless; reference number in the URL |
| Validation | Server validators + optional client script | DataAnnotations + business rules, re-checked server-side |
| Reuse | User controls (`.ascx`), master pages | Design-system components + shared JS |
| Testability | Hard: page lifecycle + controls | Easy: service + API tests in `tests/` |

When to maintain vs. modernize is covered in `docs/LEGACY_MODERNIZATION.md`.
