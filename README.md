# Inventory Reservation System

##  Problem Overview

This system simulates an inventory reservation mechanism designed to prevent **overselling** in high-concurrency environments such as flash sales. (e.g. when any new phone launch with limited stocks and flipkart / amezone start a sell for limited time then this scenario will come.)

Scenario:

* Limited stock (e.g., 1 item)
* Hundreds of concurrent users attempting to reserve simultaneously

Goal:
Ensure that **only available stock can be reserved**, even under concurrent access.

---

## Approach

The system is designed using **Clean Architecture principles** with clear separation of concerns:

* **Domain Layer** → Core business entities and rules
* **Application Layer** → Business logic and orchestration
* **Infrastructure Layer** → In-memory data storage
* **Tests** → Unit and concurrency validation

---

## Key Design Decisions

### 1. Reservation-Based Model

Instead of directly reducing stock, the system introduces a **reservation mechanism**:

* Reservation temporarily holds inventory
* Must be confirmed or expires after a fixed duration

---

### 2. Stock Calculation Strategy

Available stock is calculated dynamically:

```
Available = TotalStock - ActiveReservations - ConfirmedReservations
```

This avoids inconsistencies caused by storing derived values.

---

### 3. Encapsulation in Domain Entities

Business rules are enforced inside entities:

* Only active reservations can be confirmed or cancelled
* Expiry handled through entity behavior

---

### 4. Concurrency Handling

To prevent race conditions:

* A **lock** is used to ensure thread-safe operations
* Reservation logic is executed as an atomic operation

Tested using:

* `Parallel.For` to simulate 500 concurrent requests

---

## Testing Strategy

### Unit Tests

* Reservation success when stock is available
* Reservation failure when stock is exhausted
* Expiry releases inventory
* Confirmation updates state correctly

### Concurrency Test

Simulated 500 concurrent requests:

* Expected: **1 success, 499 failures**
* Verified using thread-safe counters

---

## Trade-offs & Improvements

### Current Approach

* Uses in-memory storage
* Uses in-process locking

### Improvements for Production

* Replace lock with **distributed locking** (e.g., Redis)
* Use database-level concurrency control (optimistic/pessimistic locking)
* Add background job for expiry cleanup
* Add API layer for real-world usage

---

## How to Run

```bash
dotnet build
dotnet test
```

---

## Project Structure

```
IRS.API/
  ├── Propties/
  ├── Controller/
  ├── Program.cs
  ├── appsetting.json //will add for configuration
  

IRS.Domain/
  ├── Entities/
  ├── Enums/
  ├── Interfaces/
  ├── Response/

IRS.Application/
  ├── Interfaces/
  ├── Services/

IRS.Infrastructure/
  ├── Repositories/

IRS.Tests/
  ├── Unit/
  ├──── Services/
  
```

##  Highlights

- Prevents overselling under high concurrency  
- Thread-safe using fine-grained per-item locking  
- Built with Clean Architecture  
- Exposes REST APIs with Swagger for testing  
- Supports full reservation lifecycle (Active → Confirmed / Expired)  

## 📡 API Layer

The system exposes REST APIs using ASP.NET Core Web API.

### Endpoints

- `GET /api/reservations/sample-id` → to Create new guid to pass in next api  
- `POST /api/reservations/{itemId}` → Create reservation  
- `POST /api/reservations/{reservationId}/confirm` → Confirm reservation  

Swagger is enabled for easy testing.

## Testing via Swagger

Run the application:

```bash
dotnet run

Open Swagger UI:

https://localhost:<port>/swagger


Steps:
Call sample-id - to get Guid 
Call Reserve API → get reservationId
Call Confirm API using same ID

This demonstrates:

No overselling
Correct reservation lifecycle

---

##  Summary

This solution focuses on:

* Correctness under concurrency
* Clean and maintainable design
* Clear separation of responsibilities
* Test-driven validation of critical scenarios

---
