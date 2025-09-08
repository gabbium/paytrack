# Paytrack

![GitHub last commit](https://img.shields.io/github/last-commit/gabbium/paytrack)
![Sonar Quality Gate](https://img.shields.io/sonar/quality_gate/gabbium_paytrack?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Coverage](https://img.shields.io/sonar/coverage/gabbium_paytrack?server=https%3A%2F%2Fsonarcloud.io)

---

## 📌 About

**Paytrack** is a modular monolith built with **.NET 9** to help users track expenses and incomes.  
It follows clean architecture principles with a clear separation of concerns and a pragmatic developer experience.

---

## 🚀 Endpoints

### 🔑 Authentication / Users

- `POST /api/users/register` → Register a new user
- `POST /api/users/login` → Authenticate and retrieve JWT

### 💸 Movements

- `POST /api/movements` → Create movement
- `GET /api/movements?pageNumber=&pageSize=` → List movements with pagination
- `GET /api/movements/{id}` → Get movement by Id
- `PUT /api/movements/{id}` → Update movement
- `DELETE /api/movements/{id}` → Delete movement

---

## 🏛️ Data Model

### User

- **Id**
- **Email** (unique)
- **PasswordHash**
- **Currency** (ISO 4217, e.g., BRL)
- **TimeZone** (IANA, e.g., America/Sao_Paulo)
- Audit: `CreatedOn`, `CreatedBy`, `LastModifiedOn`, `LastModifiedBy`

### Movement

- **Id**
- **Kind** (Expense | Income)
- **Amount** (positive integer, cents)
- **Description** (optional, up to **128** chars)
- **OccurredOn** (UTC, ISO 8601)
- Audit: `CreatedOn`, `CreatedBy`, `LastModifiedOn`, `LastModifiedBy`

---

## 🔒 Business Rules

- All operations are **scoped per user** based on JWT claims (EF Global Filter).
- **Input validation** enforced on user-provided data (e.g., amount must be > 0, required fields, etc.).
- Ownership enforced: resources not belonging to the user return **404**.

---

## 📂 Repository Structure

```
/.github/
/backend/
  /src/
    Paytrack.Api/
    Paytrack.Application/
    Paytrack.Domain/
    Paytrack.Infrastructure/
  /tests/
    Paytrack.Api.FunctionalTests/
    Paytrack.Application.UnitTests/
    Paytrack.Domain.UnitTests/
  /infra/
    .env
    .env.example
    docker-compose.yml
  Paytrack.sln
```

---

## 🛠️ Technologies

- **.NET 9**
- **Entity Framework Core** with Global Filters
- **PostgreSQL**
- **JWT Bearer Authentication**
- **Docker Compose** (local environment)
- **Serilog + Seq** for logging

### ⚙️ Architecture

- **Messaging pattern** with Commands, Queries, and Events
- **Pipeline behaviors** (logging, validation with `AbstractValidator`)
- **Result pattern** for consistent success/error handling
- **Minimal API** endpoints

### 🧪 Testing

- **Unit tests** using the **Builder pattern** (Application & Domain)
- **Functional tests** for the API using `WebApplicationFactory`
- **Testcontainers + Respawn** to manage PostgreSQL lifecycle and clean state
- **Gherkin/BDD-style** scenarios for behavior coverage

---

## 🧱 Infrastructure

The local environment is provisioned via **Docker Compose** and includes the services the application relies on:

- **PostgreSQL** — database; credentials are read from the local **`.env`** file.
- **Seq** — centralized log server for **Serilog**.
- **Portainer** — lightweight UI for container management.

---

## 🪪 License

This project is licensed under the MIT License – see [LICENSE](LICENSE) for details.
