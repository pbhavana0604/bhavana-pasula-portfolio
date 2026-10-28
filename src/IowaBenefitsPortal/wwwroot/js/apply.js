// apply.js — mirrors the server rules exactly (see Dtos/CreateApplicationRequest):
// names required, 5-digit ZIP, household 1-20, income >= 0, >= 1 service, consent.
// Client validation is a convenience only; the server re-validates everything.
(function () {
  "use strict";
  var form = document.getElementById("application-form");
  if (!form) return;

  var summary = document.getElementById("error-summary");
  var summaryList = document.getElementById("error-summary-list");
  var result = document.getElementById("form-result");

  function setError(fieldId, message) {
    var field = document.getElementById(fieldId);
    var error = document.getElementById(fieldId + "-error");
    if (field) field.setAttribute("aria-invalid", "true");
    if (error) error.textContent = message;
    return { fieldId: fieldId, message: message, label: field ? field.labels[0].textContent : fieldId };
  }
  function clearError(fieldId) {
    var field = document.getElementById(fieldId);
    var error = document.getElementById(fieldId + "-error");
    if (field) field.setAttribute("aria-invalid", "false");
    if (error) error.textContent = "";
  }

  function validate() {
    var errors = [];
    ["first-name", "last-name", "city", "zip-code"].forEach(clearError);
    clearError("household-size"); clearError("monthly-income"); clearError("consent");

    var first = document.getElementById("first-name").value.trim();
    var last = document.getElementById("last-name").value.trim();
    var city = document.getElementById("city").value.trim();
    var zip = document.getElementById("zip-code").value.trim();
    var size = Number(document.getElementById("household-size").value);
    var income = document.getElementById("monthly-income").value;
    var services = Array.prototype.slice.call(form.querySelectorAll('input[name="services"]:checked'));
    var consent = document.getElementById("consent").checked;

    if (!first) errors.push(setError("first-name", "Enter your first name."));
    if (!last) errors.push(setError("last-name", "Enter your last name."));
    if (!city) errors.push(setError("city", "Enter your city or town."));
    if (!/^\d{5}$/.test(zip)) errors.push(setError("zip-code", "Enter a 5-digit ZIP code."));
    if (!Number.isInteger(size) || size < 1 || size > 20) errors.push(setError("household-size", "Household size must be between 1 and 20."));
    if (income === "" || Number(income) < 0) errors.push(setError("monthly-income", "Monthly income must be 0 or more."));
    if (services.length === 0) {
      errors.push({ fieldId: "service-child-care", message: "Select at least one service.", label: "Services" });
      document.getElementById("services-error").textContent = "Select at least one service.";
    } else {
      document.getElementById("services-error").textContent = "";
    }
    if (!consent) errors.push(setError("consent", "Confirm that the information you entered is for this demonstration only."));
    return errors;
  }

  form.addEventListener("submit", function (event) {
    event.preventDefault();
    result.innerHTML = "";
    var errors = validate();
    if (errors.length > 0) {
      summary.hidden = false;
      summaryList.innerHTML = "";
      errors.forEach(function (e) {
        var li = document.createElement("li");
        var a = document.createElement("a");
        a.href = "#" + e.fieldId;
        a.textContent = e.message;
        li.appendChild(a);
        summaryList.appendChild(li);
      });
      summary.focus(); // Error recovery: focus moves to the summary, links jump to fields.
      window.announce(errors.length + " errors found. Please correct the form.");
      return;
    }
    summary.hidden = true;

    var payload = {
      firstName: document.getElementById("first-name").value.trim(),
      lastName: document.getElementById("last-name").value.trim(),
      city: document.getElementById("city").value.trim(),
      zipCode: document.getElementById("zip-code").value.trim(),
      householdSize: Number(document.getElementById("household-size").value),
      monthlyIncome: Number(document.getElementById("monthly-income").value),
      serviceIds: Array.prototype.slice.call(form.querySelectorAll('input[name="services"]:checked')).map(function (c) { return c.value; }),
      consent: true
    };

    fetch("/api/applications", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    }).then(function (response) {
      return response.json().then(function (body) { return { ok: response.ok, status: response.status, body: body }; });
    }).then(function (r) {
      if (!r.ok) {
        var detail = (r.body.errors && r.body.errors.join(" ")) || r.body.title || "The application could not be submitted.";
        result.innerHTML = '<div class="alert alert-error" role="alert"><h2>Submission not accepted</h2><p>' + detail + "</p></div>";
        window.announce("Submission not accepted. " + detail);
        return;
      }
      var ref = r.body.reference;
      result.innerHTML = '<div class="alert alert-success" role="status"><h2>✓ Application received (demo)</h2>' +
        "<p>Your demo reference number is <strong>" + ref + "</strong>.</p>" +
        "<p>Demo eligibility check: <strong>" + r.body.eligibilityOutcome + "</strong> — " + (r.body.eligibilityReasons || []).join(" ") + "</p>" +
        '<p><a href="status.html?ref=' + encodeURIComponent(ref) + '">Check this application status</a></p></div>';
      window.announce("Application received. Your demo reference number is " + ref + ".");
      form.reset();
    }).catch(function () {
      result.innerHTML = '<div class="alert alert-error" role="alert"><h2>Could not reach the server</h2><p>Please check that the application is running and try again.</p></div>';
      window.announce("Could not reach the server.");
    });
  });
})();
