# NFL Betting Trends

A web app (https://nflbettingprojectwebapp-bhg8f4e9aahharea.westus2-01.azurewebsites.net/) for browsing NFL game results and spread trends, and for logging your own bets against the spread — including a nightly background job that automatically settles bet payouts once games are final.

## What it does

- Browse historical games (scores, spread, total) with searchable, sortable tables for both games and teams
- Visualize how often the home team covers the spread across different spread ranges, in a bar chart
- Sign in with a Microsoft account to place bets (game, bet size, odds) and see a running list with total payout
- Every night, a background job looks at each user's open bets, checks the final score of the matching game, and updates the payout automatically — no manual settlement

## Architecture

The solution is split into three projects, following a layered design:

| Project | Role |
|---|---|
| `NflBettingTrends` | Razor Pages web app — the UI. Pages are thin: they call into the service layer and bind the result. |
| `NflBettingTrends.Shared` | Class library shared between the web app and the Functions app — EF Core entities, the `DbContext`, and the business services/interfaces both apps depend on. |
| `NflBettingTrends.Functions` | Azure Functions (isolated worker) app that runs the background jobs, independent of the web app. |

The pattern used throughout: **Entity** (EF Core model) → **Service behind an interface** (queries/business logic) → **Page or Function** (thin, just calls the injected service). Everything is wired up with constructor-injected DI (`IGamesService`, `ITeamsService`, `IUserBetsService`, `IServiceBusService`, `IGameResultsProcessor`), registered per-app in each project's `Program.cs`.

## How it uses Azure

**Azure SQL Database** — all game, team, and bet data lives in an Azure SQL database (serverless tier), accessed through EF Core via `NflDbContext`. Both apps enable `EnableRetryOnFailure()` on the SQL Server provider, so a request that hits the database while it's auto-paused/resuming retries automatically instead of failing outright.

**Razor Pages** — the web UI is built with ASP.NET Core Razor Pages (no MVC controllers, aside from the auth endpoints below), Bootstrap for layout/styling, Plotly.js for the spread-coverage chart, and a small vanilla-JS helper that makes any table's columns sortable client-side.

**Microsoft Authentication & permissions** — sign-in is handled by `Microsoft.Identity.Web` against Microsoft Entra ID (Azure AD). Authorization is policy-based:
- `AdminOnly` (requires the `Admin` role) — gates the game-entry page
- `BetTracker` (any signed-in user) — gates the bet placement/tracking page

Each bet is tied to the signed-in user's Entra object ID (`Oid`), read from their auth claims server-side — never trusted from client input.

**Azure Functions — Timer Trigger** — `NightlyPublishUserOids` runs once a night on a NCRONTAB schedule (`0 0 2 * * *`, pinned to Eastern time via the `TZ` app setting) and publishes one message per user who has an open bet.

**Azure Service Bus** — a queue (`game-results`) decouples "figure out who needs settling" from "go settle them." The timer function is the producer; a second function is the consumer, so a spike or slow settlement run never blocks the nightly scan.

**Azure Functions — Service Bus Trigger** — `ProcessGameResult` picks up each queued message, loads that user's bets (joined with their games and teams), and recalculates `Payout` from the final score, the spread, and the bet's odds and side — skipping any game that hasn't been played yet.

**Function App hosting** — `NflBettingTrends.Functions` deploys to its own Azure Function App on a Linux Consumption plan (pay-per-execution), completely separate from the web app's App Service. Background jobs run independently of web traffic and don't share compute with the site.

## Project structure

```
NflBettingTrends/
├── NflBettingTrends/            # Razor Pages web app
│   ├── Pages/                   # Games, Teams, Bets, Home
│   ├── Services/                # IGamesService, ITeamsService
│   └── Program.cs               # Auth, DI, middleware
├── NflBettingTrends.Shared/     # Shared class library
│   ├── Entities/                # EF Core models (Game, Team, UserBet, ...)
│   ├── Data/                    # NflDbContext
│   ├── Interfaces/ & Services/  # IUserBetsService, IServiceBusService, IGameResultsProcessor
│   └── Models/                  # DTOs (insert models, Service Bus message shape)
├── NflBettingTrends.Functions/  # Azure Functions (isolated worker)
│   └── Functions/               # NightlyFunction (timer), GameResultsFunction (Service Bus)
└── NflBettingTrends.sln
```

## Running locally

1. Set your Azure SQL and Service Bus connection strings in `NflBettingTrends/appsettings.Development.json` and `NflBettingTrends.Functions/local.settings.json`.
2. Web app: `dotnet run --project NflBettingTrends/NflBettingTrends.csproj`
3. Functions app (requires [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local)): `func start` from inside `NflBettingTrends.Functions/`

## Deployment

The web app and the Function App are deployed independently, to separate Azure resources:

```bash
# Web app -> Azure App Service
cd NflBettingTrends && bash deploy.sh

# Functions -> Azure Function App
cd NflBettingTrends.Functions && func azure functionapp publish <function-app-name>
```
