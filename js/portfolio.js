// Portfolio landing page behaviour: scroll progress, rotating hero word,
// and accessible technology filtering. No data is collected, and nothing
// here depends on mouse input alone — every control is a real button.
(function () {
  "use strict";

  // Scroll progress bar (decorative; aria-hidden in the markup).
  var progress = document.getElementById("pf-progress");
  function updateProgress() {
    if (!progress) return;
    var max = document.documentElement.scrollHeight - window.innerHeight;
    progress.style.width = max > 0 ? (window.scrollY / max * 100) + "%" : "0%";
  }
  window.addEventListener("scroll", updateProgress, { passive: true });
  window.addEventListener("resize", updateProgress);
  updateProgress();

  // Rotating hero word. Stops entirely under prefers-reduced-motion,
  // and the word itself is aria-hidden so screen readers hear the
  // complete headline without a changing word interrupting them.
  var rotating = document.getElementById("pf-rotating");
  var words = ["accessible", "enterprise", "scalable", "modern"];
  if (rotating && !window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
    var i = 0;
    window.setInterval(function () {
      i = (i + 1) % words.length;
      rotating.textContent = words[i];
    }, 2600);
  }

  // Technology filters: buttons with aria-pressed, cards hidden with
  // the hidden attribute (not display:none alone), and a polite live
  // region announcing how many categories are shown.
  var buttons = document.querySelectorAll("[data-pf-filter]");
  var cards = document.querySelectorAll("[data-pf-category]");
  var live = document.getElementById("pf-filter-live");
  buttons.forEach(function (button) {
    button.addEventListener("click", function () {
      var filter = button.getAttribute("data-pf-filter");
      buttons.forEach(function (b) {
        b.setAttribute("aria-pressed", b === button ? "true" : "false");
      });
      var shown = 0;
      cards.forEach(function (card) {
        var show = filter === "all" || card.getAttribute("data-pf-category") === filter;
        card.hidden = !show;
        if (show) shown += 1;
      });
      if (live) {
        live.textContent = "Showing " + shown + " technology categories.";
      }
    });
  });
})();
