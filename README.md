# Inventory Reservation System

## 📌 Overview

This project is a backend system for managing inventory and reservations using Clean Architecture.

## 🧱 Tech Stack

* .NET 8 Web API
* Entity Framework Core
* SQL Server
* Clean Architecture (Domain, Application, Infrastructure)

## 🚀 Features

* Inventory Management
* Reservation System

## 📂 Project Structure

* IRS.API → Entry point
* IRS.Application → Business logic
* IRS.Domain → Entities & core rules
* IRS.Infrastructure → DB & external services

## ⚙️ Setup Instructions

1. Clone the repository
2. Open solution in Visual Studio
3. Run the API project

## Current Progress

- Implemented domain entities with validation
- Implemented reservation service with basic concurrency control
- Added reservation lifecycle (Active, Confirmed, Expired)
- Unit tests for core scenarios

> Further improvements will include concurrency simulation and advanced handling.

## 👨‍💻 Author

Swapnil Pisal
