# PetCare

PetCare is a veterinary clinic management system that provides an API, web app for staff, and a cross-platform mobile app for pet owners. 
It includes modules for appointments, pets, owners, vets, medical records, inventory, billing, and notifications.

**Highlights**
- Modular architecture: clean separation into API, Application (Logic), Infrastructure (EF Core), WebApp (Razor Pages), and MobileApp (MAUI).
- Ready for local development with `dotnet` and containerized deployment via `docker-compose`.
- Cross-Platform Mobile: Native mobile experience built with .NET MAUI Blazor Hybrid for Android and iOS.
- Includes EF Core migrations and an uploads folder for medical assets.

**Tech stack**
- Backend: ASP.NET Core Web API (.NET 8)
- Frontend: ASP.NET Core Razor Pages (WebApp)
- Mobile: .NET MAUI Blazor Hybrid (MobileApp)
- Database: SQL Server (accessed via EF Core)
- Containerization: Docker / Docker Compose

**Default Credentials (Demo Data)**
The system initializes with the following demo accounts. Password for all member accounts: Pass123!
- Admin: admin@petcare.pl, Password: StrongPassword123!, Full access to web app
- Vet (Surgeon): john.doe@petcare.com, Staff member (Web Panel access)
- Vet (Dermatologist): sarah.smith@petcare.com, Staff member (Web Panel access)
- Vet (General Practitioner): emily.white@petcare.com, Staff member (Web Panel access)
- Client: alice@petcare.com, Standard client with a healthy dog
- Client: bob@petcare.com, Client with unpaid overdue invoices

**Prerequisites**
- .NET 8 SDK (for local development)
- Docker & Docker Compose (for containerized run)
- Visual Studio 2022 (recommended for MAUI development)

Getting started

Option 1: Run with Docker Compose (Recommended)
This will spin up the SQL Server, API, and Web App automatically.

1. Clone the repository:

```
git clone <repository-url>
cd PetCare
```
2. Run the system:

```
docker-compose up --build
```
3. Access the services:
Web App (Clinic Panel): http://localhost:5001
API (Swagger UI): http://localhost:5084/swagger

Option 2: Run Locally (Dev Mode)

1. Ensure you have a running SQL Server instance and update the connection string in appsettings.json.

2. Run the API locally:

```
dotnet run --project PetCare.Api
```

3. Restore and build with `dotnet`:

```
dotnet restore
dotnet build
```

**Mobile App (MAUI) Setup**
The mobile application cannot be run inside Docker. It must be deployed to an emulator or physical device via Visual Studio.

1. Open PetCare.sln in Visual Studio.
2. Set PetCare.MobileApp as the startup project.
3. Ensure the API is running (via Docker or locally).
	Note for Android Emulator: The app is pre-configured to connect to 10.0.2.2:5084 (which maps to localhost).
4. Run on Android Emulator or Windows Machine.

Uploads and static files

User-uploaded medical assets are stored in the `PetCare_Uploads/medicaltests` folder at the repository root. Ensure this folder is writable by the runtime when running in containers or on a host.

Project structure (top-level)

- `PetCare.Api` — RESTful API endpoints, JWT Auth, Middleware.
- `PetCare.WebApp` — Razor Pages application for clinic staff (Admin/Vets).
- `PetCare.MobileApp` — .NET MAUI Blazor Hybrid mobile client
- `PetCare.Application` — Business logic, CQRS/Services, Validation.
- `PetCare.Core` — domain models and enums
- `PetCare.Infrastructure` — Data access, EF Core migrations, services, seeding
- `PetCare.Shared` — DTOs shared between projects
- `docker-compose.yml` — Orchestration for Database, API, and WebApp.