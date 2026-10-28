// app.js — shared, framework-free behaviour: dialog + live-region announcements.
// No mouse-only interactions: dialogs are native <dialog> elements, so
// Escape closes them and focus returns to the invoking button natively.
(function () {
  "use strict";

  // Announce a message to screen readers via the page's aria-live region.
  window.announce = function (message) {
    var region = document.getElementById("live-region");
    if (!region) return;
    region.textContent = "";
    window.setTimeout(function () { region.textContent = message; }, 50);
  };

  // Disclosure buttons: button text + aria-expanded stay in sync.
  document.querySelectorAll("[data-disclosure]").forEach(function (button) {
    button.addEventListener("click", function () {
      var target = document.getElementById(button.getAttribute("aria-controls"));
      if (!target) return;
      var expanded = button.getAttribute("aria-expanded") === "true";
      button.setAttribute("aria-expanded", String(!expanded));
      target.hidden = expanded;
    });
  });

  // Dialog open/close pairs, wired by data attributes (no inline handlers).
  document.querySelectorAll("[data-dialog-open]").forEach(function (button) {
    button.addEventListener("click", function () {
      var dialog = document.getElementById(button.getAttribute("data-dialog-open"));
      if (dialog && typeof dialog.showModal === "function") dialog.showModal();
    });
  });
  document.querySelectorAll("[data-dialog-close]").forEach(function (button) {
    button.addEventListener("click", function () {
      var dialog = button.closest("dialog");
      if (dialog) dialog.close();
    });
  });
})();
