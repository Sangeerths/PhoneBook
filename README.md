# PhoneBook

A console-based contact management application built with **C# / .NET**, **Spectre.Console** for a rich terminal UI, and **Entity Framework Core** with SQL Server LocalDB for persistence.

![Language](https://img.shields.io/badge/language-C%23-239120)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![UI](https://img.shields.io/badge/UI-Spectre.Console-00ADD8)

## Features

- **Insert Contact** — Add a new contact with first/last name, organization, job title, notes, one or more phone numbers, and one or more email addresses, each with a label (Mobile, Home, Work, Other).
- **Delete Contact** — Search by name and remove a contact (with confirmation prompt).
- **Modify Contact** — Search by name and update any combination of fields, including adding new phone numbers or email addresses.
- **View Contact** — Search by name and display full contact details.
- **View All Contacts** — List every contact in a formatted table.
- **Input Validation** — Names, phone numbers, and email addresses are validated before being accepted.
- **Styled Terminal UI** — Color-coded headers, panels, and tables powered by Spectre.Console.

## Tech Stack

| Layer | Technology |
|---|---|
| UI | [Spectre.Console](https://spectreconsole.net/) |
| Data Access | Entity Framework Core |
| Database | SQL Server LocalDB |
| Language | C# (.NET) |

## Project Structure

```
PhoneBook/
├── Controller/
│   └── ContactController.cs      # Orchestrates service calls, handles success/error UI
├── Services/
│   └── ContactService.cs         # EF Core data access (CRUD operations)
├── Repository/
│   └── PhoneBookContext.cs       # EF Core DbContext
├── Models/
│   ├── Contact.cs
│   ├── PhoneNumberDetail.cs
│   └── EmailDetail.cs
├── Validation/
│   └── InputValidator.cs         # Name, phone, and email validation
├── UI/
│   ├── ConsoleMenu.cs             # Main menu flow and prompts
│   └── ConsoleUI.cs               # Reusable UI elements (headers, panels, messages)
├── Migrations/                    # EF Core migrations
└── Program.cs                     # Application entry point
```

## Data Model

A `Contact` has:
- First name, last name *(required)*
- Organization, job title, notes *(optional)*
- One or more `PhoneNumberDetail` entries (number + label)
- One or more `EmailDetail` entries (email address + label)

Labels are one of: `Mobile`, `Home`, `Work`, `Other`.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (10.0 or later)
- SQL Server LocalDB (included with Visual Studio, or installed via [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads))

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/Sangeerths/PhoneBook.git
   cd PhoneBook
   ```

2. Apply EF Core migrations to create the database:
   ```bash
   dotnet ef database update
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

The app connects to LocalDB using the connection string configured in `PhoneBookContext`:
```
Server=(localdb)\mssqllocaldb;Database=PhoneBookDb;Trusted_Connection=True;
```

## Usage

On launch, you'll see the main menu with the following options:

- **Insert Contact** — Walks you through entering contact details, phone numbers, and emails.
- **Delete Contact** — Search by first/last name, then confirm deletion.
- **Modify Contact** — Search for a contact, then choose which fields to update.
- **View Contact** — Search for and display a single contact.
- **View All Contacts** — Display every contact in the database.
- **Exit** — Close the application.

