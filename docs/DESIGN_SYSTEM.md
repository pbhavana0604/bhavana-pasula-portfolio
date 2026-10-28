# Enterprise design system — what it is, and how this demo uses one

## Three different things (the interview distinction)

| | CSS framework (e.g. Bootstrap) | UI component library (e.g. Material) | Enterprise design system |
|---|---|---|---|
| What it is | Ready-made classes/utilities you apply in markup | Ready-made components you import | Tokens + components + **rules**: naming, accessibility, usage, versioning, governance |
| Who decides | The framework authors | The library authors | The organisation, across teams |
| Changing the look | Override classes per page (drifts) | Theme props, within the library's model | Change a token; every team inherits it |
| Accessibility | Yours to get right per usage | Mostly handled per component | Contract: every component ships with keyboard, focus, ARIA and contrast rules |

Bootstrap and Material are used in the author's resume experience
and in the Iowa environment described in the posting; neither is
*the* design system. A design system may be *built on* either, but
it adds the shared tokens, the component contract, documentation,
and the adoption path. This demo proves the distinction by shipping
a small system of its own rather than only dropping in a framework.

## This demo's system

- **Tokens** (`frontend/scss/_tokens.scss`): colours, type, spacing
  scale, radius, breakpoints — as Sass variables **and** maps, so
  components look values up instead of hard-coding hex codes.
- **Mixins** (`_mixins.scss`): `focus-ring`, `respond-to($name)`,
  `button($bg, $fg)`, `card` — behaviour shared by components.
- **Base** (`_base.scss`): element defaults only.
- **Components** (`_components.scss`): buttons, secondary buttons,
  cards, card grid, fields, alerts. Live on `design-system.html`.
- **Rules**: one `h1` per page, real labels, visible focus, icon +
  text status, 4.5:1 contrast minimum — enforced by the smoke test
  and the accessibility checklist, not by hope.

## Adopting an organisation-wide system without rewriting pages

1. **Freeze the contract, not the pixels:** agree the component
   names and props (e.g. `.btn`, `.alert-error`, dialog behaviour)
   first; teams keep using those names.
2. **Re-point tokens:** replace this demo's `_tokens.scss` values
   with the organisation's tokens. Pages that only use components
   and tokens change appearance with no markup edits.
3. **Strangle, don't rewrite:** new pages use the system; existing
   pages migrate component by component (buttons first, then forms,
   then tables), with the old classes deprecated, not deleted, until
   usage drops to zero.
4. **Version and changelog:** a breaking token/component change
   gets a major version and a migration note, like any API.
5. **Keep the accessibility contract in the component:** a team
   that adopts `.alert-error` gets `role="alert"` and the contrast
   pair with it — it cannot accidentally ship a colour-only error.

## Sass techniques demonstrated

Variables, maps (`$colors`, `$spacing`, `$breakpoints`), map lookups
(`map-get`), mixins with parameters, `@content` blocks
(`respond-to`), and nesting kept to one level for states only.
Deep nesting, `@extend` chains and `!important` are avoided on
purpose: they are the usual reasons enterprise stylesheets become
unmaintainable.
