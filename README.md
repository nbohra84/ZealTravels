# ZealTravels - Travel Management System

A comprehensive travel management system built with ASP.NET Core 8.0, following Clean Architecture principles. The system supports B2B, B2C, Corporate, and White Label travel booking operations.

## 🏗️ Project Structure

The solution follows Clean Architecture with the following projects:

- **ZealTravel.Front.Web** - Customer-facing web application (B2C/B2B)
- **ZealTravel.Backoffice.Web** - Administrative backoffice application
- **ZealTravel.Application** - Application layer (Commands, Queries, Handlers)
- **ZealTravel.Domain** - Domain layer (Entities, Interfaces, Services)
- **ZealTravel.Infrastructure** - Infrastructure layer (Repositories, External Services, Database Context)
- **ZealTravel.Common** - Shared utilities and helpers

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (Express or higher)
- [Node.js](https://nodejs.org/) (v14 or higher) - For frontend asset bundling
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd ZealTravels
```

### 2. Install Node.js Dependencies

```bash
npm install
```

### 3. Configure Database Connection

1. Copy the example configuration files:
   ```bash
   # For Backoffice
   copy ZealTravelWebsite\ZealTravel.Backoffice.Web\appsettings.example.json ZealTravelWebsite\ZealTravel.Backoffice.Web\appsettings.json
   
   # For Frontend
   copy ZealTravelWebsite\ZealTravel.Front.Web\appsettings.example.json ZealTravelWebsite\ZealTravel.Front.Web\appsettings.json
   ```

2. Update `appsettings.json` files with your database connection strings:
   - Update `ConnectionStrings.DefaultConnection` with your SQL Server details
   - Update `SmtpSettings` with your email service credentials
   - Configure other service endpoints as needed

### 4. Database Setup

1. Create a new SQL Server database
2. Run Entity Framework migrations (if applicable):
   ```bash
   cd ZealTravelWebsite/ZealTravel.Infrastructure
   dotnet ef database update
   ```
   Or restore from a database backup if provided.

### 5. Build Frontend Assets

For development:
```bash
# Frontend assets
npm run build-dev

# Backoffice assets
npm run build-dev-backoffice
```

For production:
```bash
npm run build-prod
npm run build-prod-backoffice
```

### 6. Run the Application

#### Option A: Using Visual Studio
1. Open `ZealTravelWebsite/ZealTravelWebsite.sln`
2. Set startup projects:
   - Right-click solution → Properties → Multiple startup projects
   - Set `ZealTravel.Front.Web` and `ZealTravel.Backoffice.Web` to "Start"
3. Press F5 to run

#### Option B: Using .NET CLI

**Frontend Application:**
```bash
cd ZealTravelWebsite/ZealTravel.Front.Web
dotnet run
```

**Backoffice Application:**
```bash
cd ZealTravelWebsite/ZealTravel.Backoffice.Web
dotnet run
```

The applications will be available at:
- Frontend: `https://localhost:5001` (or port specified in launchSettings.json)
- Backoffice: `https://localhost:5002` (or port specified in launchSettings.json)

## 🔧 Configuration

### Application Settings

Key configuration sections in `appsettings.json`:

- **ConnectionStrings**: Database connection string
- **SmtpSettings**: Email service configuration
- **Company**: Company information and branding
- **Cache**: Cache expiration settings
- **SiteURL**: Base URL configuration
- **Airline Services**: API endpoints for airline integrations (Akasa, Galileo, Spicejet)

### Environment-Specific Settings

- `appsettings.json` - Base configuration (committed to repository)
- `appsettings.Development.json` - Development overrides (gitignored)
- `appsettings.Production.json` - Production overrides (gitignored)

**⚠️ Important:** Never commit sensitive data (passwords, connection strings, API keys) to the repository. Use environment variables or secure configuration management in production.

## 📦 Dependencies

### Backend (.NET)
- ASP.NET Core 8.0
- Entity Framework Core 8.0
- AutoMapper 13.0.1
- log4net 3.0.0
- JWT Authentication
- Swagger/OpenAPI

### Frontend (Node.js)
- Webpack 5
- Sass/SCSS
- jQuery Validation
- Babel

## 🏛️ Architecture

The solution follows **Clean Architecture** principles:

```
┌─────────────────────────────────────┐
│   ZealTravel.Front.Web              │  ← Presentation Layer
│   ZealTravel.Backoffice.Web         │
└─────────────────────────────────────┘
           ↓
┌─────────────────────────────────────┐
│   ZealTravel.Application            │  ← Application Layer
│   (CQRS: Commands, Queries, Handlers)│
└─────────────────────────────────────┘
           ↓
┌─────────────────────────────────────┐
│   ZealTravel.Domain                 │  ← Domain Layer
│   (Entities, Interfaces, Services)   │
└─────────────────────────────────────┘
           ↓
┌─────────────────────────────────────┐
│   ZealTravel.Infrastructure         │  ← Infrastructure Layer
│   (Repositories, External APIs, DB) │
└─────────────────────────────────────┘
           ↓
┌─────────────────────────────────────┐
│   ZealTravel.Common                 │  ← Shared Utilities
└─────────────────────────────────────┘
```

## 🔐 Security Considerations

1. **Never commit sensitive data** - All passwords, connection strings, and API keys should be in environment variables or secure configuration
2. **Use HTTPS in production** - Ensure SSL/TLS certificates are properly configured
3. **Database credentials** - Use Windows Authentication or secure SQL credentials
4. **API Keys** - Store airline API keys securely, never in source control

## 🧪 Development Workflow

1. Create a feature branch from `main`/`develop`
2. Make your changes
3. Ensure all configurations are updated (use example files as templates)
4. Test locally
5. Commit changes (excluding sensitive files)
6. Create a pull request

## 📝 Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML comments for public APIs
- Keep methods focused and single-purpose

## 🐛 Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string format
- Ensure database exists and user has proper permissions

### Frontend Assets Not Loading
- Run `npm install` to ensure dependencies are installed
- Run `npm run build-dev` to rebuild assets
- Clear browser cache

### Build Errors
- Restore NuGet packages: `dotnet restore`
- Clean and rebuild solution
- Verify .NET 8.0 SDK is installed

## 📚 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## 👥 Team Collaboration

### Before Pushing to GitHub

✅ **Checklist:**
- [ ] All sensitive data removed from `appsettings.json`
- [ ] `appsettings.example.json` files created and updated
- [ ] `.gitignore` properly configured
- [ ] No binary files (DLLs) in repository (unless necessary)
- [ ] README.md is up to date
- [ ] Code compiles without errors
- [ ] No hardcoded credentials or API keys

### Setting Up for New Team Members

1. Clone the repository
2. Run `npm install`
3. Copy `appsettings.example.json` to `appsettings.json`
4. Configure database connection
5. Restore database or run migrations
6. Build and run the application

## 📄 License

[Specify your license here]

## 🤝 Contributing

[Add contribution guidelines here]

---

**Note:** This project contains airline integration code. Ensure you have proper API access and credentials before attempting to use airline booking features.

