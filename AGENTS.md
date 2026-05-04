# CGym Project Notes

## Project Shape
- `CGym.slnx` is a .NET solution targeting `net10.0`.
- `CGym.Domain` contains EF/domain entities such as `User`, `Member`, `Admin`, `Trainer`, `Activity`, and `Booking`.
- `CGym.Application` contains service interfaces and application services. Keep business flow here when it is not HTTP- or EF-specific.
- `CGym.Infrastructure` contains EF Core persistence, repositories, migrations, and infrastructure services such as email.
- `CGym.API` is the ASP.NET Core Web API. Controllers live in `CGym.API/Controllers`, DI is wired in `Program.cs`, JWT auth is enabled, and Scalar/OpenAPI is mapped in Development.
- `CGym.Frontend` is the Blazor frontend. Razor pages live under `Components/Pages`, layout under `Components/Layout`, DTO/client models under `Models`, and API clients under `Services`.

## Common Commands
- Build everything: `dotnet build CGym.slnx`
- Run API locally: `dotnet run --project CGym.API/CGym.API.csproj --launch-profile http`
- Run frontend locally: `dotnet run --project CGym.Frontend/CGym.Frontend.csproj --launch-profile http`
- API HTTP launch URL: `http://localhost:5259`
- Frontend HTTP launch URL: `http://localhost:5200`
- Add an EF migration: `dotnet ef migrations add <Name> --project CGym.Infrastructure --startup-project CGym.API`
- Apply EF migrations: `dotnet ef database update --project CGym.Infrastructure --startup-project CGym.API`

## Configuration
- API configuration expects `ConnectionStrings:DefaultConnection`, `Jwt:Key`, `FrontendUrl`, and `Email:*` settings.
- Do not commit real connection strings, JWT keys, SMTP passwords, or other secrets.
- The frontend reads `ApiBaseUrl` and defaults to `http://localhost:5259/` when absent.

## Coding Conventions
- Follow the existing layered pattern: API controller -> application service/interface -> repository -> EF `GymDbContext`.
- Keep controllers thin. Put reusable behavior in application services and data access in repositories.
- Use async EF Core calls (`ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`) consistently.
- Add or update EF migrations whenever entity shape or `GymDbContext` mapping changes.
- Keep nullable reference types in mind; prefer initialized properties, nullable annotations, or `required` where appropriate.
- Use the existing block-scoped namespace style unless a file already follows a different local style.
- Admin-only API routes generally use `[Authorize(Roles = "Admin")]`.

## Verification
- There are currently no test projects in the solution, so `dotnet build CGym.slnx` is the baseline verification command.
- For frontend changes, run the frontend alongside the API and verify the relevant Razor page in a browser.
- For API changes, exercise the controller endpoints manually or through Scalar/OpenAPI in Development when practical.
