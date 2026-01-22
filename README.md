# 🚀 Task Management System (Kanban Board)

A robust, Trello-like task management application built with **ASP.NET Core** following **Clean Architecture** principles. It features a dynamic Kanban board with drag-and-drop capabilities, allowing users to organize tasks efficiently.

![Project Status](https://img.shields.io/badge/Status-In%20Development-yellow)
![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![License](https://img.shields.io/badge/License-MIT-green)

## 📸 Screenshots

<img width="1024" height="575" alt="image" src="https://github.com/user-attachments/assets/637f8640-61f1-4592-a4bf-5410a0c8d5e7" />
<img width="1024" height="575" alt="image" src="https://github.com/user-attachments/assets/f2846d75-0d57-48bd-a3a0-ba62685b7f4b" />
<img width="1024" height="575" alt="image" src="https://github.com/user-attachments/assets/12032b72-348b-4681-ba1d-78e535695b2e" />

## ✨ Key Features

* **Dynamic Kanban Board:** Create and manage multiple task groups.
* **Drag & Drop Interface:** Smoothly move tasks between sections using Vanilla JavaScript API.
* **Clean Architecture:** Strict separation of concerns to ensure scalability.
* **Task Management:** Create, edit, delete, and assign tasks to users.
* **Rich Task Details:** Modal views for editing task descriptions, priorities, and deadlines.
* **Responsive Design:** Optimized for various screen sizes.

## 🏗️ Architecture

The solution follows **Clean Architecture** rules to ensure scalability and testability. The dependency flow is directed towards the Domain layer.

* **DomainLayer:** Contains enterprise logic and entities (POCOs). No dependencies.
* **ApplicationLayer:** Contains business logic and orchestration.
    * Includes `DTOs`, `Interfaces`, `Mappings`, and `Services`.
    * Depends on DomainLayer.
* **DataAccessLayer:** Handles database interactions and configuration.
    * Includes `Context`, `Configurations` (EF Core), `Implementations` (Repositories), and `Migrations`.
    * Depends on ApplicationLayer.
* **PresentationLayer (Web MVC):** The UI layer containing `Controllers`, `Views`, and `wwwroot` resources.
    * Depends on ApplicationLayer and DataAccessLayer (via DI).
* **TaskManagementSystem.Tests:** Contains unit and integration tests for the solution.

## 🛠️ Tech Stack

* **Backend:** ASP.NET Core 9.0 (MVC)
* **Database:** Entity Framework Core (SQL Server)
* **Frontend:** HTML5, CSS3, Vanilla JavaScript (No heavy frameworks)
* **Styling:** Bootstrap 5, FontAwesome
* **Tools:** Visual Studio / Rider, SSMS

## 🚀 Getting Started

Follow these steps to run the project locally.

### Prerequisites
* [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
* SQL Server (LocalDB or Express)

### Installation

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/yourusername/task-management-system.git](https://github.com/yourusername/task-management-system.git)
    ```

2.  **Configure Database**
    Open `appsettings.json` in the **PresentationLayer** and update the connection string if necessary:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskDb;Trusted_Connection=True;"
    }
    ```

3.  **Apply Migrations**
    Open your terminal in the solution folder and run the update command pointing to the **DataAccessLayer**:
    ```bash
    dotnet ef database update --project DataAccessLayer --startup-project PresentationLayer
    ```

4.  **Run the Application**
    ```bash
    dotnet run --project PresentationLayer
    ```

## 🤝 Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

## 📝 License

Distributed under the MIT License. See `LICENSE` for more information.

---
*Developed by [İbrahim Kabadayı](https://github.com/ibrahimkabadayi)*
