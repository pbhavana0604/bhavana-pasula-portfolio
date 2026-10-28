# CI/CD — Azure DevOps (primary) and GitHub Actions (alternative)

## Azure DevOps pipeline — `azure-pipelines.yml`

Stages: restore → build (Release) → test (with TRX results
published) → publish the web app as an artifact. It runs on
`ubuntu-latest` with .NET 8. No secrets are referenced; a real
deployment stage would add an Azure App Service deployment task
using a service connection (not a password in YAML).

## GitHub Actions — `.github/workflows/ci.yml`

The same steps (restore, build, test, publish artifact) for a
GitHub-hosted repository, so the portfolio can be reviewed and
built in either system. Both pipelines are checked in; only one
needs to be used.

## Future AWS path (not implemented, no cloud resources created)

If the organisation moves toward AWS, the equivalent would be:
CodePipeline → CodeBuild (restore/build/test, same `dotnet`
commands) → artifact to ECR → ECS/Fargate (or App Runner) with
the connection string from AWS Secrets Manager, CloudWatch logs,
and an ALB with TLS. The application code needs no change for
that move: it reads configuration from the environment and has
no Azure-specific dependency. This is a sketch for discussion,
not a deployed pipeline.
