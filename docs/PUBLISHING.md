# Publishing this portfolio — exact steps

The author publishes this herself; no account of hers is accessed
or used by this build. No personal contact details are in the repo.

## 1. Create the repository

```bash
cd iowa-doit-portfolio
git init
git add .
git commit -m "Portfolio: fictional Iowa benefits portal (ASP.NET Core 8, accessible front end)"
```

Create an empty repository on GitHub (web UI, or with the GitHub CLI):

```bash
gh repo create iowa-doit-portfolio --public --source=. --remote=origin --push
```

If `gh` is not authenticated, create the repo in the GitHub web
UI instead, then:

```bash
git remote add origin https://github.com/<your-username>/iowa-doit-portfolio.git
git branch -M main
git push -u origin main
```

The repository link is what goes on the resume / submission
(the posting requires code samples or a portfolio link upfront).

## 2. Optional: preview the static pages on GitHub Pages

GitHub Pages serves static files only — it can preview
`src/IowaBenefitsPortal/wwwroot/` (the landing page, design
system and forms' markup/CSS/JS), but the API calls will fail
there because there is no server. To preview just the pages:

```bash
# In the repo settings: Pages → deploy from branch → /docs is not used here.
# Simplest honest option: copy wwwroot to a gh-pages branch.
git checkout --orphan gh-pages
git rm -rf .
cp -r src/IowaBenefitsPortal/wwwroot/* .
git add . && git commit -m "Static preview (no API)" && git push origin gh-pages
```

Label that preview clearly as "static preview — API not included".

## 3. Optional: host the full app (manual, requires the author's account)

- **Azure App Service (recommended for this stack):** create an
  App Service (.NET 8), deploy the pipeline artifact from
  `azure-pipelines.yml` via a service connection, set
  `ConnectionStrings__IowaBenefits` in App Service configuration
  (or Key Vault reference) only if a database is wanted; the app
  runs without it. Enable HTTPS-only and diagnostic logging.
- **Container:** `docker build` from a Dockerfile the author adds
  at deploy time (none is shipped, to avoid implying a tested
  image), push to ACR/ECR, run on App Service for Containers or
  ECS. Do not bake secrets into the image.

## 4. Before sharing

- Confirm the README honesty label is intact.
- Run `dotnet test` once more on the pushed commit.
- Do the manual accessibility pass in `docs/ACCESSIBILITY.md`
  (keyboard + JAWS + axe) and record the results — do not share
  a compliance claim you have not tested.
