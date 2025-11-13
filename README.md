# Task Management Service

A task management REST API built with .NET 8 and clean architecture. Features include Entity Framework, repository pattern, and a comprehensive test suite.

## Features

- **Clean Architecture** with separation of Core, Infrastructure, and API layers
- **Entity Framework Core** with repository pattern and SQL Server
- **Comprehensive Testing** with xUnit and Moq (17/17 tests passing)
- **Business Logic Validation** in the domain layer
- **RESTful API Design** following best practices

## Tech Stack

- .NET 8, ASP.NET Core, C#
- Entity Framework Core, SQL Server
- xUnit, Moq, EF Core In-Memory Database
- Clean Architecture, Repository Pattern

## Project Structure
TaskManagement/
  - Core/ # Domain entities, interfaces, business logic
  - Infrastructure/ # Data access, EF Core, repositories
  - API/ # REST API controllers, DTOs
  - Tests/ # Unit and integration tests

## Architecture

This project follows Clean Architecture principles:
- **Core**: Business entities, interfaces, domain logic
- **Infrastructure**: Data access, external services
- **API**: Presentation layer, controllers, DTOs

## Testing

- 17 unit tests covering entities and repositories
- 100% test coverage on core business logic
- In-memory database for fast, isolated testing

## Getting Started

```bash
# Clone the repository
git clone https://github.com/Danielrib1989/TaskManagementService.git

# Restore dependencies
dotnet restore

# Run tests
dotnet test

# Run the application
dotnet run --project TaskManagement.API
