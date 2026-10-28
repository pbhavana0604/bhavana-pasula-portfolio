// services.js — renders the fictional catalogue from GET /api/services.
// If the API is unreachable, the page keeps its static fallback content.
(function () {
  "use strict";
  var list = document.getElementById("service-list");
  if (!list) return;
  fetch("/api/services").then(function (r) { return r.json(); }).then(function (services) {
    list.innerHTML = "";
    services.forEach(function (s) {
      var article = document.createElement("article");
      article.className = "card";
      var h = document.createElement("h2"); h.textContent = s.name;
      var p = document.createElement("p"); p.textContent = s.summary;
      var a = document.createElement("p"); a.textContent = "For: " + s.audience;
      article.appendChild(h); article.appendChild(p); article.appendChild(a);
      list.appendChild(article);
    });
    window.announce(services.length + " fictional services loaded.");
  }).catch(function () {
    window.announce("Service list could not be loaded from the server; showing the page content as written.");
  });
})();
