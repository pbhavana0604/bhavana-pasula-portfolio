# Accessibility — WCAG 2.1 AA and Section 508

> **Honest scope.** This demo is built to WCAG 2.1 AA patterns and is
> tested as described below. **No page in this repository is claimed to
> be fully WCAG compliant or Section 508 compliant, and no accessibility
> certification is claimed.** An automated tool reporting no violations
> is not a compliance finding: automated tools catch roughly a third of
> issues; keyboard, screen-reader, zoom and reflow testing must still be
> done by a person. The author's resume records Section 508 / WCAG 2.1
> work and axe-core use at the State of Maine; it does not record a
> formal WCAG certification or formal WCAG training, and none is claimed.
The revised resume (2026-10-27) also describes accessibility
remediation at UCLA Health and the State of Maine — missing
labels, keyboard interactions, focus order, and dynamic-content
accessibility, tested with axe-core and screen-reader testing.
Those are the defect types the before/after examples below
cover; they are experience, not a credential.

## What is built in

- Semantic landmarks: `<header>`, `<nav aria-label="Primary">`,
  `<main id="main-content">`, `<footer>` on every page.
- Skip link to `#main-content`, visible on focus.
- One `<h1>` per page, headings in order, no skipped levels.
- A real `<label>` for every input; hints via `aria-describedby`,
  never placeholder-only labels (the smoke test fails if a
  `placeholder=` appears on `apply.html`).
- Error handling: an error summary (`tabindex="-1"`, focused on
  submit) with links to each field, field-level messages with
  `role="alert"`, and `aria-invalid` toggled on the field.
- Status announcements through one `aria-live="polite"` region
  (`#live-region`); success/error alerts carry `role="status"` /
  `role="alert"`.
- Status shown as icon **and** text (`.status-*` classes), never
  colour alone.
- Native `<dialog>` for dialogs: browser-managed focus, Escape to
  close, focus return. Native disclosure via `aria-expanded` +
  `hidden` for the "how it works" panel.
- Visible focus everywhere: 3px gold outline, 3px offset, never
  removed. No keyboard traps, no mouse-only interactions.
- Colour pairs (see `frontend/scss/_tokens.scss`): ink on white
  ≈13.6:1, primary on white ≈8.2:1, accent on white ≈8.9:1 —
  all above the 4.5:1 AA threshold for normal text.
- Reflow: content works at 320px width and at 200% browser zoom
  without horizontal scrolling or lost content.

## Manual testing checklist (do these by hand)

1. **Keyboard only:** Tab through every page in order; every link,
   button, input and dialog must be reachable, operable with
   Enter/Space, and show visible focus. No trap: Tab/Shift+Tab must
   always be able to leave a component.
2. **Screen reader (JAWS):** with the virtual cursor, read the page
   top to bottom; check landmarks (JAWS `;` / `R` to list regions),
   headings list (`Insert+F6`), and form fields list
   (`Insert+F5`). In forms mode, each field must announce its label,
   hint and error. Confirm the error summary is announced on a failed
   submit and the confirmation reference is announced on success.
3. **Focus management:** on a failed submit, focus moves to the error
   summary; on dialog open, focus moves inside; on close, focus
   returns to the opener.
4. **Forms/errors:** submit each form empty, then with each invalid
   value (bad ZIP, household size 0, no service, no consent) and
   confirm the summary, field message and announcement all agree.
5. **Contrast:** check every text/background pair with a contrast
   analyser (do not trust eyeballing); check focus indicators and
   non-text UI (borders, icons) against 3:1.
6. **Reflow/zoom:** 320px viewport and 200% zoom — no horizontal
   scroll, no clipped text, no overlapping controls.
7. **Mobile:** touch targets at least 44×44 CSS px where practical;
   the demo buttons and checkboxes meet this via padding/size.
8. **WCAG criteria spot-check:** 1.3.1, 1.4.1, 1.4.3, 1.4.10, 2.1.1,
   2.1.2, 2.4.1, 2.4.3, 2.4.6, 2.4.7, 3.3.1, 3.3.2, 3.3.3, 4.1.2, 4.1.3.

## axe workflow

- **Browser (recommended):** install axe DevTools, open each page,
  run "Scan ALL of my page", and triage every issue — including
  "needs review" items, which axe cannot decide alone.
- **Command line (optional, manual step):** with the app running,
  `npx @axe-core/cli http://localhost:5111/apply.html` (requires
  network access to install the CLI; no axe run is bundled or faked
  in this repository).
- Record results in an issue tracker with page, criterion, severity
  and fix; re-run after each fix. A clean axe run is recorded as
  "axe: no violations on [date/page]" — never as "WCAG compliant".

## JAWS workflow

1. Start JAWS, open the page, press `Insert+F6` (headings) and
   `Insert+F5` (form fields) — both lists must be complete and
   meaningful.
2. Read with the virtual cursor; switch to forms mode (`Enter` on a
   field) and confirm labels, hints (`aria-describedby`) and errors
   are announced.
3. Submit the form with errors: confirm the error summary is read
   and its links move focus to the right fields.
4. Open the dialog on the home/design-system page: confirm focus
   enters it, Escape closes it, and focus returns to the opener.
5. Note anything JAWS announces oddly (duplicate text, missing
   names, wrong reading order) as a defect even if axe passed.

## Before / after remediation examples

### 1. Missing label — broken vs. fixed
```html
<!-- BROKEN: placeholder is not a label; it disappears on input. -->
<input type="text" placeholder="First name">
<!-- FIXED -->
<label for="first-name">First name</label>
<input id="first-name" type="text">
```

### 2. Validation error not announced — broken vs. fixed
```html
<!-- BROKEN: red text only; screen readers may never encounter it. -->
<p style="color:red">ZIP is wrong</p>
<!-- FIXED: linked summary + field message with role="alert", focus to summary. -->
<div class="error-summary" tabindex="-1"><ul><li><a href="#zip-code">Enter a 5-digit ZIP code.</a></li></ul></div>
<p class="field-error" id="zip-code-error" role="alert">Enter a 5-digit ZIP code.</p>
```

### 3. Bad heading order — broken vs. fixed
```html
<!-- BROKEN: h1 followed by h4, skipping levels. -->
<h1>Apply</h1><h4>Your details</h4>
<!-- FIXED -->
<h1>Apply</h1><h2>Your details</h2>
```

### 4. Keyboard trap — broken vs. fixed
```javascript
// BROKEN: a custom widget that captures Tab and never releases it.
// FIXED: use a native <dialog> (browser-managed focus/Escape) or, for a
// custom widget, implement a focus loop AND an Escape/close path that
// returns focus to the opener. This demo uses native <dialog> only.
```

### 5. Inadequate contrast — broken vs. fixed
```css
/* BROKEN: #9aa on white ≈ 2.8:1 — fails AA. */
.hint { color: #999; }
/* FIXED: #33475a on white ≈ 9.7:1. */
.hint { color: #33475a; }
```

### 6. Missing accessible name — broken vs. fixed
```html
<!-- BROKEN: an icon-only button with no name. -->
<button>×</button>
<!-- FIXED: visible text where possible; otherwise aria-label. -->
<button aria-label="Close dialog">Close</button>
```

## Automated tests in this repo

`tests/IowaBenefitsPortal.Tests` includes an HTML smoke test
(skip link, `lang`, landmarks, one `h1`, labels, no placeholders).
It is a regression net, not an accessibility audit. Real axe and
JAWS runs remain manual steps above and are **not verified** in
this environment (no JAWS licence here; axe browser run not
performed — see README "Not verified").
