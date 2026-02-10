# Task Management System

A multi-layered task management application built with .NET 9.0 ASP.NET Core MVC. This system allows users to manage projects, sections, and tasks with real-time updates and an intuitive "TaskFlow" interface.

## 🚀 Features

- **Project & Task Management**: Organize work into projects, sections, and tasks.
- **TaskFlow Interface**: A streamlined view for managing and tracking tasks.
- **Real-time Updates**: Powered by SignalR for live collaboration features.
- **User Authentication**: Secure sign-in, account creation, and profile management.
- **Project Invitations**: Generate and manage invite links to bring team members into projects.
- **Email Notifications**: Integrated email service for account verification and notifications.

## 🛠️ Tech Stack

- **Language**: C# 13.0
- **Framework**: ASP.NET Core MVC (net9.0)
- **Database**: SQL Server (Entity Framework Core)
- **Real-time**: SignalR
- **Mailing**: MailKit & MimeKit
- **Security**: BCrypt.Net-Next for password hashing
- **Mapping**: AutoMapper
- **Testing**: xUnit

## 📋 Requirements

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Optional, for containerized deployment)
- IDE: Visual Studio 2022, JetBrains Rider, or VS Code

## ⚙️ Setup & Installation

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/yourusername/TaskManagementSystem.git
    cd TaskManagementSystem
    ```

2.  **Database Configuration**:
    Update the connection string in `PresentationLayer/appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDB;Database=TaskManagementDB;Trusted_Connection=True;..."
    }
    ```

3.  **Email Configuration**:
    Configure your SMTP settings in `PresentationLayer/appsettings.json` under `EmailSettings`.

4.  **Apply Migrations**:
    ```bash
    dotnet ef database update --project DataAccessLayer --startup-project PresentationLayer
    ```

5.  **Run the Application**:
    ```bash
    dotnet run --project PresentationLayer
    ```

## 🐳 Docker Support

You can also run the entire system (Application + SQL Server) using Docker:

1.  **Build and Start Containers**:
    ```bash
    docker-compose up -d
    ```

2.  **Access the Application**:
    Open [http://localhost:5000](http://localhost:5000) in your browser.

3.  **Stop Containers**:
    ```bash
    docker-compose down
    ```

*Note: The first run might take a few minutes as it pulls images and builds the application.*

## 📜 Scripts & Commands

- **Build Solution**: `dotnet build`
- **Run Tests**: `dotnet test`
- **Add Migration**: `dotnet ef migrations add <MigrationName> --project DataAccessLayer --startup-project PresentationLayer`
- **Update Database**: `dotnet ef database update --project DataAccessLayer --startup-project PresentationLayer`

## 🔑 Environment Variables & Configuration

The application uses `appsettings.json` for configuration:

| Section | Key | Description |
| :--- | :--- | :--- |
| `ConnectionStrings` | `DefaultConnection` | SQL Server connection string |
| `EmailSettings` | `SmtpServer` | SMTP server address |
| `EmailSettings` | `SmtpPort` | SMTP port |
| `EmailSettings` | `Username` | SMTP username |
| `EmailSettings` | `Password` | SMTP password (use App Passwords for Gmail) |

## 🧪 Testing

Unit tests are located in the `TaskManagementSystem.Tests` project.
To run all tests:
```bash
dotnet test
```

## 📂 Project Structure

```text
TaskManagementSystem/
├── ApplicationLayer/      # Business logic, DTOs, Mappings, and Services
├── DataAccessLayer/       # EF Core Context, Migrations, and Implementations
├── DomainLayer/           # Entities, Enums, and Interfaces
├── PresentationLayer/     # ASP.NET Core MVC (Controllers, Views, Hubs, wwwroot)
└── TaskManagementSystem.Tests/ # Unit tests (xUnit)
```

## 📄 License

TODO: Add license information (e.g., MIT, Apache 2.0).

---

*Generated/Updated on: 2026-02-10*
