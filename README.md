# Inventory Management System (IMS) — Backend API

Production-ready backend API demonstrating enterprise architecture and scalable inventory management using ASP.NET Core 8, Clean Architecture, and CQRS.

This system is designed for real-world business use and freelance deployment scenarios.

---

## Overview

The Inventory Management System (IMS) Backend is a RESTful API that provides complete inventory and warehouse management functionality.

Core capabilities include:

* Product management
* Warehouse and location management
* Inventory transactions (receive, issue, adjust, transfer)
* User and role management
* Audit tracking
* Reporting and stock movement history

The system follows enterprise-grade architecture patterns and is fully containerized using Docker.

---

## Architecture

This project follows Clean Architecture principles to ensure scalability, maintainability, and separation of concerns.
```
IMS
├── IMS.Domain → Core business entities and rules
├── IMS.Application → CQRS handlers, interfaces, validation, business logic
├── IMS.Infrastructure → EF Core, Identity, persistence, external integrations
└── IMS.Api → Controllers, middleware, authentication, configuration
```
---

## Request Flow
```
Client
↓
API Controller (Thin Controller)
↓
MediatR Command or Query
↓
Application Layer (Business Logic)
↓
Infrastructure Layer (Database Access)
↓
SQL Server Database
```
---

## Technology Stack

### Backend

* ASP.NET Core 8 Web API
* C#
* MediatR (CQRS pattern)
* Entity Framework Core
* ASP.NET Identity
* FluentValidation

### Database

* SQL Server

### Security

* JWT Authentication
* Role-based Authorization
* SecurityStamp validation for token revocation

### DevOps

* Docker
* Docker Compose
* Health Checks
* Environment-based configuration

---

## Core Features

### Authentication and Authorization

* Secure JWT-based authentication
* ASP.NET Identity integration
* Role-based authorization system

Supported roles:

* Admin
* Manager
* Clerk
* Auditor

Security hardening includes:

* Token revocation using SecurityStamp
* Immediate token invalidation when:

  * Password changes
  * Roles change
  * User is deactivated

---

### User Management (Admin)

Admin users can:

* View users
* Assign roles
* Activate and deactivate users
* Reset passwords

---

### Product Management

* Create products
* Update products
* Activate and deactivate products
* Track product stock history
* View product timeline

---

### Warehouse and Location Management

* Create warehouses
* Manage warehouse locations
* Track stock per warehouse and location

---

### Inventory Transactions

Supports full inventory lifecycle:

* Receive stock
* Issue stock
* Transfer stock between warehouses
* Adjust stock

All operations are fully auditable.

---

### Reporting

Provides reporting capabilities:

* Stock movement reports
* Product timeline reports
* Inventory summaries

---

### Audit and Traceability

Tracks:

* CreatedAt / UpdatedAt timestamps
* CreatedBy / UpdatedBy users
* Complete inventory movement history

Ensuring full system traceability.

---

## API Endpoints (Example)

### Authentication
```
POST /api/auth/login
POST /api/auth/register
```
---

### Products
```
GET /api/products
GET /api/products/{id}
POST /api/products
PUT /api/products/{id}
PUT /api/products/{id}/activate
PUT /api/products/{id}/deactivate
```
---

### Warehouses
```
GET /api/warehouses
GET /api/warehouses/{id}
POST /api/warehouses
PUT /api/warehouses/{id}
```
---

### Locations
```
GET /api/locations
POST /api/locations
```
---

### Inventory
```
POST /api/inventory/receive
POST /api/inventory/issue
POST /api/inventory/transfer
POST /api/inventory/adjust

GET /api/inventory/movements
```
---
## API Documentation

### Swagger Overview

![Swagger Overview](docs/Swagger-Overview/1.png)

### Authentication Endpoint

![Auth Login](docs/swagger-auth-login.png)

### Products Endpoints

![Products](docs/swagger-products.png)

### Inventory Endpoints

![Inventory](docs/swagger-inventory.png)

### Docker Deployment

![Docker](docs/docker-running.png)
---
## Enterprise Patterns Used

### Clean Architecture

Benefits:

* Separation of concerns
* High maintainability
* Scalable structure
* Testable design

---

### CQRS with MediatR

Separates:

* Commands (write operations)
* Queries (read operations)

Benefits:

* Clean business logic
* Improved scalability
* Better organization

---

### Result Pattern

Uses Result<T> for consistent API responses.

Benefits:

* Standardized responses
* Predictable error handling

---

### MediatR Pipeline Behaviors

Implemented behaviors:

* LoggingBehavior
* ValidationBehavior
* TransactionBehavior
* CachingBehavior
* CacheInvalidationBehavior

---

### FluentValidation

Validation handled in Application layer.

Benefits:

* Thin controllers
* Centralized validation
* Clean architecture compliance

---

## Security Features

* JWT Authentication
* Role-based Authorization
* SecurityStamp validation
* Global exception handling middleware
* Setup key protection for initialization

---

## Health Checks

Endpoints:
```
GET /health/live
GET /health/ready
```
Used for monitoring and deployment readiness.

---

## Docker Deployment

Fully containerized system.

Services:

* ims-api
* ims-sql

Run using:

docker compose up --build

---

## Environment Configuration

Supported environment variables:
```
ConnectionStrings__ImsConnection
ConnectionStrings__AuthConnection
```
```
Jwt__Issuer
Jwt__Audience
Jwt__Secret
Jwt__ExpiryMinutes
```
```
Database__AutoMigrate
```
```
SETUP_KEY
```
---

## Setup Initialization

Secure initialization endpoint allows creation of:

* Roles
* First Admin user

Protected using Setup Key.

---

## How to Run Locally

1. Clone repository

git clone https://github.com/YOUR_USERNAME/inventory-management-api.git

2. Navigate to project folder

cd inventory-management-api

3. Run using Docker

docker compose up --build

4. Open Swagger UI

http://localhost:8080/swagger

---

## API Design Principles

* Thin controllers
* Business logic in Application layer
* CQRS separation
* Consistent response structure
* RESTful API design

---

## Production Readiness

This backend is production-ready with:

* Secure authentication
* Scalable architecture
* Docker deployment
* Health monitoring
* Audit tracking
* Role-based authorization

---

## Future Enhancements

Possible extensions:

* Angular frontend integration
* Redis caching
* Cloud deployment (Azure / AWS)
* CI/CD pipeline

---

## Author
```
Hazem Mohamed Anter
ASP.NET Core Backend Developer
Specialized in Clean Architecture and REST API development
```
---

## License

This project is intended for educational, portfolio, and professional use.
