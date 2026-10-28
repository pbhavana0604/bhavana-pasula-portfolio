// Portfolio landing page behaviour: scroll progress, rotating hero word,
// scroll reveal, and accessible technology filtering. No data is collected,
// and nothing here depends on mouse input alone — every control is a real
// button. Reveal is skipped entirely under prefers-reduced-motion, and the
// CSS keeps revealed elements visible in that case as a fallback.
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

  // Scroll reveal: sections, cards and stats fade/slide in as they enter
  // the viewport. Content is never hidden from assistive technology —
  // only its opacity/position animates — and reduced-motion users skip it.
  if (!window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
    var revealTargets = document.querySelectorAll(".pf-section, .pf-card, .pf-stat");
    if ("IntersectionObserver" in window) {
      var observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
          if (entry.isIntersecting) {
            entry.target.classList.add("is-visible");
            observer.unobserve(entry.target);
          }
        });
      }, { threshold: 0.15 });
      revealTargets.forEach(function (target) {
        target.classList.add("pf-reveal");
        observer.observe(target);
      });
    }
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
