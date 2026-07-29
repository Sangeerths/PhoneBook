# PhoneBook

A console-based contact management application built with **C# / .NET**, **Spectre.Console** for a rich terminal UI, and **Entity Framework Core** with SQL Server LocalDB for persistence.

![Language](https://img.shields.io/badge/language-C%23-239120)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![UI](https://img.shields.io/badge/UI-Spectre.Console-00ADD8)

## Table of Contents
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Data Model](#data-model)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
- [Configuration](#configuration)
  - [Mailgun Setup (Email)](#mailgun-setup-email)
  - [Twilio Setup (SMS)](#twilio-setup-sms)
- [Usage](#usage)

## Features
- **Insert Contact** — Add a new contact with first/last name, organization, job title, notes, one or more phone numbers, and one or more email addresses, each with a label (Mobile, Home, Work, Other). Shows a review summary and asks for confirmation before saving.
- **Delete Contact** — Search by name and remove a contact, with a confirmation prompt showing who will be deleted.
- **Modify Contact** — Search by name and update any combination of fields, including adding new phone numbers or email addresses. Shows current details before editing and updated details before confirming the save.
- **View Contact** — Search by name and display full contact details.
- **View All Contacts** — List every contact in a formatted table.
- **Send Message** — Search for a contact and send them a message via Email or SMS, using their stored contact info.
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
├── Messaging/
│   └── MessageSender.cs          # Sends messages via Email/SMS channel
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

## Configuration

The **Send Message** feature sends emails via [Mailgun](https://www.mailgun.com/) and SMS via [Twilio](https://www.twilio.com/). Both require an account and a set of credentials, which the app reads from environment variables (see `.env.sample` in the repo — copy it to `.env` and fill in your own values, or set these as actual environment variables on your machine).

| Variable | Used for |
|---|---|
| `MailgunApiKey` | Your Mailgun private API key |
| `MailgunDomain` | Your Mailgun sending domain |
| `EmailFrom` | The "From" address emails are sent from |
| `TwilioSid` | Your Twilio Account SID |
| `TwilioToken` | Your Twilio Auth Token |
| `SmsFrom` | The Twilio phone number messages are sent from |

### Mailgun Setup (Email)
1. Create a free account at [mailgun.com](https://www.mailgun.com/).
2. In the Mailgun dashboard, go to **Sending → Domains**. A sandbox domain (e.g. `sandboxXXXXXXX.mailgun.org`) is provided automatically for testing — no DNS setup required, but you can only send to pre-authorized recipients on the free sandbox domain (add them under **Authorized Recipients**).
3. For production use, add and verify your own domain instead (requires adding DNS records Mailgun provides — SPF, DKIM, etc.).
4. Go to **Settings → API Keys** and copy your **Private API key** → this is `MailgunApiKey`.
5. Set `MailgunDomain` to the domain shown in Sending → Domains (sandbox or your verified domain).
6. Set `EmailFrom` to an address on that domain, e.g. `Excited User <mailgun@your-domain.mailgun.org>`.

### Twilio Setup (SMS)
1. Create a free account at [twilio.com](https://www.twilio.com/).
2. On the [Twilio Console dashboard](https://console.twilio.com/), copy your **Account SID** and **Auth Token** → these are `TwilioSid` and `TwilioToken`.
3. Get a phone number capable of sending SMS: go to **Phone Numbers → Manage → Buy a number** (trial accounts get a free number with SMS capability).
4. Set `SmsFrom` to that number in E.164 format, e.g. `+15551234567`.
5. Note: on a Twilio **trial account**, SMS can only be sent to phone numbers you've manually verified under **Phone Numbers → Manage → Verified Caller IDs**. Upgrade the account to send to any number.

> ⚠️ Treat all of these credentials as secrets. Never commit a real `.env` file — only commit `.env.sample` with placeholder values.

## Usage
On launch, you'll see the main menu with the following options:
- **Insert Contact** — Walks you through entering contact details, phone numbers, and emails, then confirms before saving.
- **Delete Contact** — Search by first/last name, then confirm deletion.
- **Modify Contact** — Search for a contact, choose which fields to update, review the changes, then confirm.
- **View Contact** — Search for and display a single contact.
- **View All Contacts** — Display every contact in the database.
- **Send Message** — Search for a contact, choose Email or SMS, and send a message using their stored details.
- **Exit** — Close the application.
