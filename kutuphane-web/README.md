# Kütüphane Yönetim Sistemi

A library management web application built with **ASP.NET Core 8 (MVC + Web API)**,
**Entity Framework Core (Code First)** and **PostgreSQL**.

The project is split into a REST API and an MVC front-end that consumes it through
`HttpClient` — the same architecture taught in the *Web Tabanlı Programlama* course
(weeks 7 to 15).

---

## Features

- 📚 **Books** — full CRUD, grouped by category, with live "available copies" count
- 🏷️ **Categories** — full CRUD (a category with books cannot be deleted)
- 👤 **Members** — full CRUD, with active-loan count
- 🔄 **Loans** — lend a book to a member and return it; a book can only be lent
  while a free copy exists (business rule enforced by the API)
- 📖 **Swagger / OpenAPI** documentation for the API
- 🌱 **Seed data** so the app has content on first run

---

## Architecture

```
Browser
  │
  ▼
Kutuphane.Web  (ASP.NET Core MVC)
  │   calls the API with a typed HttpClient (LibraryApiClient)
  ▼
Kutuphane.Api  (ASP.NET Core Web API)
  │   EF Core (Code First) + Repository pattern
  ▼
PostgreSQL

Kutuphane.Shared  →  DTOs shared by the Web and the API
```

| Project | Responsibility |
|---------|----------------|
| **Kutuphane.Api** | REST API, EF Core `DbContext`, entities, repositories, migrations, Swagger |
| **Kutuphane.Web** | MVC site (Razor views + Bootstrap) that consumes the API |
| **Kutuphane.Shared** | DTOs (data transfer objects) referenced by both projects |

---

## Tech stack

- .NET 8 / C#
- ASP.NET Core MVC & Web API
- Entity Framework Core 8 (Code First, migrations)
- PostgreSQL (via `Npgsql.EntityFrameworkCore.PostgreSQL`)
- Bootstrap 5 for the UI
- Swagger (Swashbuckle) for API documentation

### What it covers from the course

| Week | Topic | Where in the project |
|------|-------|----------------------|
| 7 | EF Core, ORM, Code First | `Models/`, `Data/KutuphaneContext.cs`, `Migrations/` |
| 8 | CRUD (add / update / delete) | API controllers + MVC create/edit/delete views |
| 11 | Template, partials, Repository, Bootstrap | `Views/Shared/_Navbar`, `_Footer`, `Repositories/` |
| 12 | Model binding | MVC forms bound to input DTOs |
| 13–14 | REST, HTTP methods, Swagger | `Controllers/*`, Swagger UI |
| 15 | Consuming a Web API with `HttpClient` | `Kutuphane.Web/Services/LibraryApiClient.cs` |

---

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (version 14 or newer)
- Visual Studio 2022 (or `dotnet` CLI / VS Code)

### 1. Set up PostgreSQL

The default connection string (in `Kutuphane.Api/appsettings.json`) is:

```
Host=localhost;Port=5432;Database=kutuphanedb;Username=kutuphane;Password=kutuphane123
```

Create a matching login role (the database itself is created automatically on first run):

```sql
CREATE ROLE kutuphane WITH LOGIN PASSWORD 'kutuphane123' CREATEDB;
```

> Prefer to use your own PostgreSQL account? Just edit the `DefaultConnection`
> string in `Kutuphane.Api/appsettings.json` to match your username/password.
> Switching database engine is a one-line change (`UseNpgsql` → e.g. `UseSqlServer`),
> which is the "database independence" advantage of an ORM mentioned in week 7.

### 2. Run the project

The database schema and seed data are **created automatically** the first time the
API starts (EF Core `Database.Migrate()`), so there is no manual migration step.

**In Visual Studio 2022**

1. Open `Kutuphane.sln`.
2. Right-click the solution → **Set Startup Projects…** → **Multiple startup projects**,
   and set both **Kutuphane.Api** and **Kutuphane.Web** to **Start**.
3. Press **F5**.

**With the .NET CLI** (two terminals)

```bash
dotnet run --project Kutuphane.Api    # API
dotnet run --project Kutuphane.Web    # Web site
```

### 3. Open it

| App | URL |
|-----|-----|
| Web site | https://localhost:7090 |
| API (Swagger) | https://localhost:7080/swagger |

> The Web site reads the API base URL from `ApiBaseUrl` in
> `Kutuphane.Web/appsettings.json` (default: `http://localhost:5080/`).

### Running migrations manually (optional)

If you want to manage migrations yourself (as shown in week 7):

```powershell
# Visual Studio → Package Manager Console (Default project: Kutuphane.Api)
Add-Migration MigrationName
Update-Database
```

```bash
# or with the CLI
dotnet ef migrations add MigrationName --project Kutuphane.Api
dotnet ef database update --project Kutuphane.Api
```

---

## Project structure

```
kutuphane-web/
├── Kutuphane.sln
├── Kutuphane.Api/
│   ├── Controllers/        # BooksController, CategoriesController, MembersController, LoansController
│   ├── Data/               # KutuphaneContext (DbContext + relationships + seed)
│   ├── Models/             # Category, Book, Member, Loan (EF Core entities)
│   ├── Repositories/       # Generic Repository<T> + entity-specific repositories
│   ├── Migrations/         # EF Core migrations
│   └── Program.cs
├── Kutuphane.Web/
│   ├── Controllers/        # MVC controllers (call the API)
│   ├── Services/           # LibraryApiClient (typed HttpClient)
│   ├── Views/              # Razor views + Shared/_Navbar, _Footer
│   └── Program.cs
└── Kutuphane.Shared/
    └── Dtos/               # CategoryDto, BookDto, MemberDto, LoanDto, ...
```

---

## API endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/books` | List books (with available copies) |
| GET | `/api/books/{id}` | Get one book |
| POST | `/api/books` | Create a book |
| PUT | `/api/books/{id}` | Update a book |
| DELETE | `/api/books/{id}` | Delete a book |
| GET/POST/PUT/DELETE | `/api/categories` | Category CRUD |
| GET/POST/PUT/DELETE | `/api/members` | Member CRUD |
| GET | `/api/loans` | List loans |
| POST | `/api/loans` | Lend a book to a member |
| POST | `/api/loans/{id}/return` | Return a loan |

---

## Design decisions

- **PostgreSQL** — chosen as the database engine (recommended for the course);
  EF Core keeps the code identical to any other provider.
- **Code First** — the schema is described in C# and generated through migrations,
  so anyone can recreate the database from scratch.
- **Repository pattern** — a generic `Repository<T>` provides basic CRUD; entity-specific
  repositories (`BookRepository`, `LoanRepository`, …) add eager loading and the
  borrow/return rules.
- **DTOs** — the API never exposes EF entities directly; it maps them to DTOs that also
  carry computed fields (available copies, active loan count).
- **Two projects + HttpClient** — the MVC site does not touch the database; it only
  talks to the API, exactly like a real front-end / back-end split.

---

## Data model

```
Category (1) ──────< (∞) Book
                          │ (1)
                          │
                          ∧ (∞)
Member (1) ──────< (∞)  Loan
```

- A **Category** has many **Books**.
- A **Book** has many **Loans**; a **Member** has many **Loans**.
- A **Loan** links one Book and one Member; `ReturnDate` is `null` while the book is out.
