# Architecture Overview

High-level overview of Paytrack’s architecture and patterns.

---

## Style: Modular Monolith (.NET 9)

- Single deployable unit with clear **modules/layers**:
  - **Paytrack.Api** – Minimal API endpoints, composition root, middleware.
  - **Paytrack.Application** – Use cases (Commands/Queries/Handlers), validation, behaviors, contracts.
  - **Paytrack.Domain** – Entities, value objects, business rules, exceptions.
  - **Paytrack.Infrastructure** – EF Core, repositories, persistence (Outbox), integrations.

---

## Core Patterns

- **Messaging pattern**: Commands, Queries, Events
  - Handlers return a **Result**-like type for consistent success/error semantics.
- **Pipeline behaviors**:
  - **Logging** (timings/outcomes)
  - **Validation** using `AbstractValidator`
- **EF Core**:
  - **Global filter** for user scoping (no RLS in v1).
  - **Auditing** via `SaveChanges` interceptor.
- **Outbox (v1)**:
  - Events are persisted in the same transaction as data changes.
  - Publication to brokers is deferred to **v1.1** (worker).
- **Minimal API**:
  - Lightweight endpoint definitions with clear request/response contracts.

---

## Security & Scope

- **JWT Bearer** auth; `sub` contains UserId.
- All data access is restricted by **EF Global Filter** and **owner checks** (404 if not owned).

---

## Testing Strategy

- **Unit tests** with Builder pattern, Shouldly, Moq.
- **Functional tests** with WebApplicationFactory, Testcontainers + Respawn, BDD-style (Steps/Scenarios).
