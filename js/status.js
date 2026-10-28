// status.js — looks up a demo reference via GET /api/applications/{reference}.
// Only demo fields are shown; no sensitive data is requested or displayed.
(function () {
  "use strict";
  var form = document.getElementById("status-form");
  if (!form) return;
  var result = document.getElementById("status-result");
  var input = document.getElementById("reference");

  function render(data) {
    var statusClass = data.status === "NeedsInformation" ? "status-needsinfo" : "status-received";
    result.innerHTML = '<div class="card"><h2>Application ' + data.reference + '</h2>' +
      '<p><span class="status ' + statusClass + '">Status: ' + data.status + '</span> — status is shown as text, never by colour alone.</p>' +
      "<p>Demo eligibility check: <strong>" + data.eligibilityOutcome + "</strong></p>" +
      "<p>" + (data.eligibilityReasons || []).join(" ") + "</p>" +
      "<p>City: " + data.city + " · ZIP: " + data.zipCode + " · Household size: " + data.householdSize + "</p>" +
      "<p>Services: " + data.serviceIds.join(", ") + "</p></div>";
  }

  form.addEventListener("submit", function (event) {
    event.preventDefault();
    var ref = input.value.trim();
    if (!ref) { result.innerHTML = '<div class="alert alert-error" role="alert"><p>Enter a reference number, for example IA-DEMO-2026-000101.</p></div>'; return; }
    fetch("/api/applications/" + encodeURIComponent(ref)).then(function (r) {
      return r.json().then(function (body) { return { ok: r.ok, body: body }; });
    }).then(function (r) {
      if (!r.ok) {
        result.innerHTML = '<div class="alert alert-error" role="alert"><p>No application was found for that reference number.</p></div>';
        window.announce("No application was found for that reference number.");
      } else {
        render(r.body);
        window.announce("Application " + r.body.reference + " found. Status " + r.body.status + ".");
      }
    }).catch(function () {
      result.innerHTML = '<div class="alert alert-error" role="alert"><p>Could not reach the server.</p></div>';
    });
  });

  // Pre-fill from ?ref= so the confirmation link on apply.html works.
  var params = new URLSearchParams(window.location.search);
  var prefilled = params.get("ref");
  if (prefilled) { input.value = prefilled; form.dispatchEvent(new Event("submit")); }
})();
