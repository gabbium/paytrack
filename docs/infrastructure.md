# Infrastructure & Local Setup

How to run the local environment and services used by the application.

---

## .env

Create a `.env` in `/infra` based on `.env.example`. At minimum provide:

```
POSTGRES_USER=youruser
POSTGRES_PASSWORD=yourpassword
# add other variables as needed
```

The API’s connection string typically references these variables via configuration (e.g., `Host=localhost;Port=5432;Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};Database=paytrack` or equivalent in appsettings).

---

## Configuration & Secrets

Some values are present in **appsettings.json** but left empty. They must be provided either through **User Secrets** (for local dev) or **environment variables** (for Docker/production).

### Required secrets

- `Jwt:Secret` – secret key for signing JWTs.
- `ConnectionStrings:DefaultConnection` – PostgreSQL connection string.

### Setting secrets locally

Run these in the `backend/src/Paytrack.Api` project folder:

```bash
# JWT secret
dotnet user-secrets set "Jwt:Secret" "mysupersecretkey"

# Connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=paytrack;Username=youruser;Password=yourpassword"
```

> These values will override what is in `appsettings.json` during development.

In production (e.g., Docker), provide them as **environment variables** instead.

---

## Docker Compose Services

The local stack includes:

- **PostgreSQL** – application database (credentials from `.env`).
- **Seq** – centralized logging for Serilog, available at **http://localhost:5341**.
- **Portainer** – container management UI at **https://localhost:9443**.

> These match the compose file mappings: `5432:5432` (Postgres), `5341:80` (Seq), `9443:9443` (Portainer).

---

## Start / Stop

From the `/infra` directory:

```bash
# start in background
docker compose up -d

# view logs
docker compose logs -f

# stop
docker compose down
```

---

## Database Migrations (optional)

Typical EF Core workflow:

```bash
# add a migration
dotnet ef migrations add <Name> -p ./backend/src/Paytrack.Infrastructure -s ./backend/src/Paytrack.Api

# apply migrations (dev)
dotnet ef database update -p ./backend/src/Paytrack.Infrastructure -s ./backend/src/Paytrack.Api
```

> Ensure the API project is used as the startup project (`-s`) so configuration is loaded correctly.

---

## Accessing Services

- **Seq**: http://localhost:5341
  - Filter by `CorrelationId` or `UserId` properties set by the middleware.
- **Portainer**: https://localhost:9443
- **PostgreSQL**: `localhost:5432` (use the credentials from `.env`).
