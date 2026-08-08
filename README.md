# ITes Backend

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10-blueviolet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-18-blue)
![Docker](https://img.shields.io/badge/docker-ready-blue)

> Backend of **ITes** — a platform for organizing IT competitions, hackathons, freelance orders, and team building.

ITes Backend is a REST API built with ASP.NET Core that powers the ITes platform. It provides authentication, authorization, business logic, and data persistence for participants, organizers, and clients.

Originally created during a hackathon, the backend has since been significantly redesigned and expanded with a modular architecture and production-oriented practices.

## Features

### Authentication

* JWT Authentication
* Secure password hashing
* Role-based user registration
* Protected API endpoints

### Authorization

* Role-based authorization
* Permission-based authorization
* Custom Authorization Policy Provider

### Participants

* Manage user profiles
* Upload avatars
* Apply to competitions
* Apply to freelance orders
* Apply to teams
* Build a portfolio from accepted applications

### Organizers

* Create competitions and events
* Edit competition descriptions
* Review participant applications
* Approve or reject applications

### Clients

* Publish freelance orders
* Review applications
* Accept or reject candidates

### Teams

* Create teams
* Join teams
* Manage team members
* Assign participant roles

### API

* RESTful API
* Request validation
* Global exception handling
* Swagger / OpenAPI documentation

## Tech Stack

### Backend

* ASP.NET Core 10
* C# 14
* Entity Framework Core 10
* PostgreSQL 18
* JWT Authentication
* Docker
* Docker Compose

### Architecture

* Clean Architecture
* Repository Pattern
* Service Layer
* Dependency Injection
* Options Pattern

## Project Structure

```text
/
├── ites.Application/
│   ├── Contracts/
│   ├── Interfaces/
│   └── Services/
│
├── ites.Core/
│   ├── Entities/
│   ├── Enums/
│   ├── Exceptions/
│   ├── Interfaces/
│   └── Models/
│
├── ites.DataAccess/
│   ├── Configurations/
│   ├── Migrations/
│   ├── Repositories/
│   ├── AuthorizationOptions.cs
│   └── ItesDbContext.cs
│
├── ites.Infrastructure/
│   ├── Auth/
│   ├── Files/
│   └── Mapping/
│
└── ites.Server/
    ├── Controllers/
    ├── Extensions/
    ├── Filters/
    └── Program.cs
```

## Architecture

The project follows **Clean Architecture** principles.

### Core

Contains:

* Domain entities
* Enums
* Exceptions
* Repository interfaces

No external dependencies.

### Application

Contains:

* Business logic
* Service interfaces
* Application models

Depends only on **Core**.

### DataAccess

Contains:

* Entity Framework Core
* DbContext
* Repository implementations
* Database migrations

Depends only on **Core**.

### Infrastructure

Contains:

* JWT authentication
* Authorization
* Password hashing
* Object mapping
* External service implementations

Depends on **Application** and **DataAccess**.

### Server

Contains:

* API controllers
* Dependency Injection
* Authentication configuration
* Middleware
* Swagger configuration

## Authentication

The API uses JWT Bearer Authentication.

Authentication flow:

```text
Client
    │
POST /auth/login
    │
    ▼
JWT Token
    │
Authorization: Bearer <token>
    │
    ▼
Protected Endpoints
```

## Authorization

Supports:

* Roles
* Permissions
* Custom Authorization Policies

Example:

```csharp
[Authorize]
```

```csharp
[HasPermission(Permission.CreateCompetition)]
```

## Frontend

The frontend is available in a separate repository:

**[ITes Frontend](https://github.com/gurori/ites-frontend)**

## Running Locally

### Requirements

- .NET 10 SDK
- Docker (recommended)

### Installation

```bash
git clone https://github.com/gurori/ites-backend.git

cd ites-backend
```

### Running with Docker

Development:

```bash
docker compose \
    -f docker-compose.yml \
    -f docker-compose.dev.yml \
    up --build
```

Production:

```bash
docker compose \
    -f docker-compose.yml \
    -f docker-compose.prod.yml \
    up --build
```

Swagger UI:

```text
http://localhost:8080/swagger
```

## Configuration

The application is configured using:

* `appsettings.json`
* `appsettings.Development.json`

Configuration includes:

- Database connection
- JWT authentication
- CORS
- Authorization policies
- Logging

## Project Status

This project is actively maintained and continuously improved.

## About

This project demonstrates experience with:

- Clean Architecture
- ASP.NET Core Web API
- REST API design
- JWT authentication
- Role & permission-based authorization
- Entity Framework Core
- PostgreSQL
- Docker and Docker Compose
- Next.js integration

## License

All rights reserved.

The source code is publicly available for viewing and educational purposes only.
No permission is granted to use, copy, modify, distribute, or deploy this software
without explicit written permission from the copyright holder.
