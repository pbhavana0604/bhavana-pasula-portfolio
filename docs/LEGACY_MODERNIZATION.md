# Legacy maintenance and modernization

## The representative sample

`code-samples/legacy/` holds a Web Forms page
(`BenefitsLookup.aspx` + `.aspx.cs`) and a user control
(`StatusCard.ascx`), plus `ModernEquivalent.md` showing the same
flow as an ASP.NET Core API call. **These files are .NET Framework
sample code and are intentionally NOT compiled in the .NET 8
solution** — Web Forms does not run on .NET 8, and pretending
otherwise would be dishonest. They are there to be read and
discussed, which is what an interview about legacy work needs.

## Web Forms vs. modern ASP.NET Core MVC / APIs

- **Request model:** Web Forms posts the whole page back to itself;
  the server re-renders it. ASP.NET Core routes a request to a
  controller action that returns JSON (or a view), with no postback.
- **State:** Web Forms keeps control state in ViewState, a hidden
  field round-tripped on every postback — large pages get slow and
  fragile. ASP.NET Core is stateless; identity lives in the URL,
  a token, or server-side session only where needed.
- **Controls vs. markup:** Web Forms server controls abstract HTML
  away, which makes accessible markup (labels, focus, ARIA) harder
  to control precisely. Razor/HTML gives exact markup control —
  which is why the accessible pages in this demo are plain HTML.
- **Testability:** a page lifecycle with controls is hard to unit
  test; a service + controller is straightforward (see `tests/`).
- **Separation:** code-behind mixes page, validation and data
  access by default. The modern code separates DTOs, services and
  repositories, with dependency injection throughout.

## When to maintain, when to modernize

**Maintain (in place, carefully):** the page is stable, its users
depend on it, the change is small, and the risk of a rewrite
outweighs the benefit. Maintain with the same discipline as new
code: parameterized queries, server validation, accessible markup
where the control model allows, and regression tests.

**Modernize (strangler pattern):** the page needs major change,
its ViewState/postback model blocks accessibility or performance,
or it must integrate with new APIs. Extract one workflow at a time
behind the existing UI (new API + new page, old page retired when
usage moves), rather than a big-bang rewrite.

## Grounding in the author's actual experience

Per her resume, the author maintained and enhanced legacy
ASP.NET Web Forms pages at UCLA Health (bed management),
Fifth Third Bank (loan servicing) and the State of Maine
(citizen/caseworker portals), while modernizing eligibility and
benefit-processing applications toward ASP.NET Core, REST APIs
and Azure services at the State of Maine. No other state-government work is claimed; this demo's legacy sample is illustrative,
modelled on that kind of maintenance work.
