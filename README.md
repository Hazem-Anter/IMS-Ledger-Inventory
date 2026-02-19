# Inventory Management System (IMS) — Backend

A production-grade **Inventory Management System (IMS) backend** built using **ASP.NET Core 8**, **Clean Architecture**, and **CQRS with MediatR**.  
This system provides secure, scalable, and auditable inventory and warehouse management suitable for real-world business and freelance deployment.

---

## Overview

The IMS backend provides complete inventory management functionality including:

- Product management
- Warehouse and location management
- Inventory transactions (receive, issue, adjust, transfer)
- User and role management
- Audit tracking
- Reporting and stock movement history

The project follows enterprise-grade architecture patterns and is fully containerized using Docker.

---

## Architecture

This project follows **Clean Architecture** principles to ensure maintainability, scalability, and separation of concerns.

IMS
├── IMS.Domain → Core business entities and rules
├── IMS.Application → CQRS handlers, interfaces, validation, business logic
├── IMS.Infrastructure → EF Core, Identity, persistence, external integrations
└── IMS.Api → Controllers, middleware, authentication, startup


### Request Flow

Client
↓
API Controller (Thin)
↓
MediatR Command/Query
↓
Application Layer
↓
Infrastructure Layer
↓
SQL Server Database

---

## Technology Stack

### Backend
- ASP.NET Core 8 Web API
- C#
- MediatR (CQRS pattern)
- Entity Framework Core
- ASP.NET Identity
- FluentValidation

### Database
- SQL Server

### Security
- JWT Authentication
- Role-based Authorization
- SecurityStamp validation for token revocation

### DevOps
- Docker
- Docker Compose
- Health Checks
- Environment-based configuration

---

## Core Features

### Authentication and Authorization

- Secure JWT-based authentication
- ASP.NET Identity integration
- Role-based authorization system

Supported roles:

- Admin
- Manager
- Clerk
- Auditor

Security hardening includes:

- Token revocation using SecurityStamp
- Immediate token invalidation when:
  - Password changes
  - Roles change
  - User is deactivated

---

### User Management (Admin)

Admin users can:

- View users
- Assign and remove roles
- Activate and deactivate users
- Reset passwords

---

### Product Management

- Create and update products
- Activate and deactivate products
- Track product stock history
- View product timeline

---

### Warehouse and Location Management

- Create and manage warehouses
- Create and manage warehouse locations
- Track stock per warehouse and location

---

### Inventory Transactions

Supports full inventory lifecycle:

- Receive stock
- Issue stock
- Adjust stock
- Transfer stock between warehouses

All operations are fully auditable.

---

### Reporting

Provides reporting features such as:

- Stock movement reports
- Product timeline reports
- Inventory summaries

---

### Audit and Traceability

The system tracks:

- CreatedAt / UpdatedAt
- CreatedBy / UpdatedBy
- Full inventory transaction history

Ensuring full traceability.

---

## Enterprise Patterns Used

### Clean Architecture

Benefits:

- Clear separation of concerns
- Testable and maintainable
- Scalable design

---

### CQRS with MediatR

Separates:

- Commands (write operations)
- Queries (read operations)

Benefits:

- Clean and organized business logic
- Improved scalability

---

### Result Pattern

Uses `Result<T>` for consistent API responses.

Benefits:

- Standardized error handling
- Predictable API behavior

---

### Pipeline Behaviors

Implemented MediatR pipeline behaviors:

- LoggingBehavior
- ValidationBehavior
- TransactionBehavior
- CachingBehavior
- CacheInvalidationBehavior

---

### FluentValidation

Validation is handled in the Application layer.

Benefits:

- Thin controllers
- Centralized validation
- Clean architecture compliance

---

## Security Features

- JWT Authentication
- Role-based Authorization
- SecurityStamp token validation
- Global exception handling middleware
- Setup key protection for initialization

---

## Health Checks

Health endpoints:

GET /health/live
GET /health/ready

Used for monitoring and deployment readiness.

---

## Docker Deployment

The system is fully containerized.

Services:

- ims-api
- ims-sql

Run using:

```bash
docker compose up --build

Configuration

Environment variables supported:
ConnectionStrings__ImsConnection
ConnectionStrings__AuthConnection

Jwt__Issuer
Jwt__Audience
Jwt__Secret
Jwt__ExpiryMinutes

Database__AutoMigrate
SETUP_KEY
Setup Initialization

The system provides a secure initialization endpoint to create:

Roles

First Admin user

Protected using Setup Key.

API Design Principles

Thin controllers

Business logic in Application layer

CQRS separation

Clean and consistent response structure

RESTful API design

Production Readiness

This backend is production-ready with:

Secure authentication

Scalable architecture

Docker deployment

Health monitoring

Audit tracking

Role-based authorization

Future Enhancements

Possible extensions:

Angular frontend

Redis caching

Cloud deployment (Azure / AWS)

CI/CD pipeline

Author

Hazem Mohamed Anter
Full Stack Developer (.NET)

License

This project is intended for educational, portfolio, and professional use.

---

If you want, I can also create a **README with architecture diagrams and badges (Docker, .NET, SQL Server, Clean Architecture)** to look more impressive to recruiters.
