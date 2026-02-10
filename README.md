# 🚀 TaskFlow - Project Management System

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![SignalR](https://img.shields.io/badge/Real--Time-SignalR-blue?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

**TaskFlow** is a modern, high-performance project management platform designed to streamline team collaboration. Built with **Clean Architecture** principles and **.NET 9**, it features real-time updates, a dynamic Kanban board, and secure role-based access control.

---

## 📸 Screenshots

| **Dashboard** | **Interactive Kanban Board** |
|:---:|:---:|
| <img width="2559" height="1439" alt="image" src="https://github.com/user-attachments/assets/851d8d44-daf2-448a-986f-b34cb4b29953" />| <img width="2559" height="1439" alt="image" src="https://github.com/user-attachments/assets/008517a5-b40f-4dea-b55f-5020703dd942" />
| *Modern and Intuitive Dashboard Overview* | *Drag & drop tasks with live updates* |

---

## ✨ Key Features

* **⚡ Real-Time Collaboration:** Powered by **SignalR**, see task moves, assignments, and status changes instantly without refreshing the page.
* **🏗️ Clean Architecture:** Designed with separation of concerns (**Onion Architecture**), ensuring maintainability and scalability.
* **📊 Dynamic Kanban Board:** Interactive drag-and-drop interface for managing task states (To Do, In Progress, Done).
* **👥 Role-Based Access:** Granular permissions for **Leaders**, **Developers**, and **Viewers** within projects.
* **🐳 Dockerized:** Fully containerized application and database for easy deployment.
* **🔒 Secure Authentication:** Custom cookie-based authentication with secure password hashing (BCrypt).
* **📱 Responsive Design:** Modern UI built with Vanilla JS (ES6+) and CSS3 variables.

---

## 🛠️ Tech Stack

### Backend
* **.NET 9 (ASP.NET Core MVC)**
* **Entity Framework Core** (Code-First)
* **SQL Server 2022**
* **SignalR** (WebSockets)
* **Clean Architecture** (Domain, Application, DataAccess, Presentation Layers)

### Frontend
* **Vanilla JavaScript (ES6+)** - No heavy frameworks, pure performance.
* **HTML5 & CSS3** - Custom properties and flexbox/grid layouts.
* **Bootstrap 5** - For responsive grid system.

### DevOps & Tools
* **Docker & Docker Compose**
* **Git** for Version Control
* **JetBrains Rider / Visual Studio 2022**

---

## 🏗️ Architecture

The solution follows the **Onion Architecture** to ensure the core business logic remains independent of external frameworks.

```text
TaskManagementSystem/
├── 📂 DomainLayer          # Core Entities, Enums, Interfaces (No dependencies)
├── 📂 ApplicationLayer     # Business Logic, DTOs, Services, Mappings
├── 📂 DataAccessLayer      # EF Core Context, Migrations, Repositories
├── 📂 PresentationLayer    # Controllers, Views, Hubs (MVC Web App)
└── 📂 TaskManagementSystem.Tests # Unit Tests (xUnit)
```

## 🚀 Getting Started

You can run TaskFlow using **Docker** (Recommended) or locally with Visual Studio.

### Option 1: Run with Docker (Fastest) 🐳

Prerequisites: [Docker Desktop](https://www.docker.com/products/docker-desktop) installed.

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/yourusername/TaskManagementSystem.git](https://github.com/yourusername/TaskManagementSystem.git)
    cd TaskManagementSystem
    ```

2.  **Run with Docker Compose:**
    ```bash
    docker-compose up --build
    ```
    *This command will set up both the SQL Server database and the Web Application automatically. It also handles database migrations on startup.*

3.  **Access the App:**
    Open your browser and navigate to `http://localhost:5000`.

### Option 2: Run Locally (Visual Studio / Rider) 💻

1.  **Configure Database:**
    Update `appsettings.json` in `PresentationLayer` with your LocalDB string.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDB;Database=TaskManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

2.  **Apply Migrations:**
    Open Package Manager Console (or Terminal) and run:
    ```bash
    dotnet ef database update --project DataAccessLayer --startup-project PresentationLayer
    ```

3.  **Run the Project:**
    Set `PresentationLayer` as the startup project and press `F5`.

---

## 🔑 Configuration

The application uses `appsettings.json` (or Environment Variables in Docker) for configuration:

| Section | Key | Description |
| :--- | :--- | :--- |
| `ConnectionStrings` | `DefaultConnection` | SQL Server connection string |
| `EmailSettings` | `SmtpServer` | SMTP server address |
| `EmailSettings` | `SmtpPort` | SMTP port |
| `EmailSettings` | `Username` | SMTP username |
| `EmailSettings` | `Password` | SMTP password |

---

## 🧪 Testing

The project includes Unit Tests for the Application layer logic.
To run all tests:

```bash
dotnet test
```

## 🤝 Contributing

We love your input! We want to make contributing to **TaskFlow** as easy and transparent as possible, whether it's reporting a bug, discussing the current state of the code, submitting a fix, or proposing new features.

### Development Process

1.  **Fork the repo** and clone it to your local machine.
2.  **Create a branch** for your specific changes:
    ```bash
    git checkout -b feature/my-new-feature
    # or for bugs
    git checkout -b fix/bug-description
    ```

### Architectural Guidelines

Since this project follows **Clean Architecture**, please adhere to these rules:

* **Domain Layer**: Must remain pure. Do not add external dependencies or references to other layers.
* **Application Layer**: Contains business logic and interfaces. Do not reference the Presentation or DataAccess layers directly.
* **Presentation Layer**: Keep controllers thin. Move complex logic to the Application layer services.

### Submitting a Pull Request

1.  **Run Tests**: Ensure your changes don't break existing functionality.
    ```bash
    dotnet test
    ```
2.  **Commit your changes**: Use clear and descriptive commit messages.
    ```bash
    git commit -m 'Add: Real-time notification for task assignment'
    ```
3.  **Push to the branch**:
    ```bash
    git push origin feature/my-new-feature
    ```
4.  **Open a Pull Request**: Go to the repository on GitHub and open a PR against the `main` branch.

### Reporting Bugs

If you find a bug, please create an issue containing:
* A clear title and description.
* Steps to reproduce the error.
* The environment (Docker vs Local) you are using.
