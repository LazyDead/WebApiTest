# Store Statistics Web API (Test Project)

## Overview

This is a test project that demonstrates the implementation of a Web API
for reading statistical data from a relational database with an
expandable and maintainable architecture.

The main focus of the project is:
- clean separation of concerns
- interchangeable data access layers
- efficient read-only queries
- clear and predictable API behavior

---

## Goals

- Build a **Web API** to read statistical data from a database
- Design an **expandable architecture** that allows switching
  data access implementations without affecting business logic

---

## Database

A **SQLite** database is used with the following tables:

- `Clients`
- `Orders`
- `Orders_Positions`
- `Products`

The project includes:
- SQL scripts to **create the database schema**
- SQL scripts to **populate the database with test data**

The database schema is assumed to be pre-created.
The application does not modify the schema at runtime.

---

## Architecture

The project is structured into logical layers:

- **WebApi**
  - Controllers
  - HTTP endpoints

- **Application**
  - DTOs
  - Domain services
  - Repository interfaces

- **Infrastructure**
  - Entity Framework Core repository
  - ADO.NET (SQLite) repository
  - Database mappings and configuration

Business logic is isolated in a **domain service** and does not depend on
the concrete data access implementation.

---

## Data Access Strategy

Two interchangeable data access implementations are provided:

- **Entity Framework Core**
- **ADO.NET (SQLite)**

Both implementations conform to the same interface: 

```
IStatisticRepository
```

The active implementation is selected via `appsettings.json`:

```json
{
  "UseEf": true
}
```

This allows switching between EF Core and ADO.NET without changing
business logic or controllers.

## Implemented API Endpoints

### 1. Get clients by birthday

Returns clients whose birthday matches the provided date.

**Request**
```
GET /api/clients?birthday={date}
```

**Response**
- Collection of clients:
  - `id`
  - `fullName`

---

### 2. Get recent buyers

Returns clients who placed orders during the last **N days**.
For each client, the date of the most recent order is returned.

**Request**
```
GET /api/orders/recent?days={daysAmount}
```

**Response**
- Collection of clients:
  - `client`
     - `id` [int]
     - `fullName` [string]
  - `lastBuyDate` [date (dd-MM-yyyy HH:mm:ss)]

---
   
### 3. Get purchased categories by client

Returns product categories purchased by a specific client
and the total quantity of purchased products per category.

**Request**
```
GET /api/clients/{clientId}/categories
```

**Response**
- Collection of categories:
  - `categoryName`
  - `purchaseCount`

---

## Performance Considerations

- All read-only queries use `AsNoTracking()`
- Aggregations (`GROUP BY`, `SUM`, `MAX`) are executed in the database
- DTO projections are used to minimize transferred data

This ensures predictable and efficient query execution.

---

## Technologies Used

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- ADO.NET

---

## Notes

This project focuses on correctness, clarity, and architectural flexibility
rather than advanced infrastructure or deployment concerns.



        
