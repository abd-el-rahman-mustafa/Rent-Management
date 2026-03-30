# 🏠 Rent Management System

A full-stack **Rent Management** application built with **Angular 21** and **ASP.NET Core 10**. The system provides secure authentication with **two-factor authentication (2FA)** via email OTP, **multi-language support** (English & Arabic with RTL), and a clean layered architecture ready for enterprise use.

---

## 🚀 Tech Stack

### Backend

| Technology                | Version | Purpose                    |
| ------------------------- | ------- | -------------------------- |
| **ASP.NET Core**          | .NET 10 | Web API framework          |
| **Entity Framework Core** | 10.0    | ORM / Data access          |
| **SQL Server**            | —       | Relational database        |
| **ASP.NET Identity**      | 10.0    | User & role management     |
| **JWT Bearer**            | 10.0    | Token-based authentication |
| **MailKit**               | 4.15    | SMTP email delivery        |
| **AutoMapper**            | 15.1    | Object-to-object mapping   |
| **Seq**                   | 9.0     | Structured logging         |
| **OpenAPI**               | 10.0    | API documentation          |

### Frontend

| Technology       | Version | Purpose               |
| ---------------- | ------- | --------------------- |
| **Angular**      | 21.2    | SPA framework         |
| **TypeScript**   | 5.9     | Type-safe JavaScript  |
| **Tailwind CSS** | 4.2     | Utility-first styling |
| **RxJS**         | 7.8     | Reactive programming  |
| **ngx-toastr**   | 20.0    | Toast notifications   |
| **Zone.js**      | 0.15    | Change detection      |

---

## ✨ Features

### 🔐 Authentication & Security

- **User Registration** — Sign up with email, first/last name, and password
- **Two-Factor Authentication (2FA)** — Email-based OTP verification required on every login
- **OTP System** — Time-limited one-time passwords with rate limiting (60-second cooldown) and automatic invalidation of previous codes
- **JWT Authentication** — Stateless token-based auth with configurable expiry, issuer, and audience
- **Route Guards** — `authGuard` and `notAuthGuard` to protect pages based on authentication state
- **Auth Interceptor** — Automatically attaches JWT tokens to outgoing API requests
- **Role-Based Access Control** — Roles (e.g., Admin) with seeded default users and roles

### 📧 Email Configuration

- **SMTP Integration** — Powered by **MailKit** with STARTTLS support
- **Configurable Provider** — SMTP relay via Brevo (or any SMTP provider) configured in `appsettings.json`
- **OTP Emails** — Automatic delivery of verification codes for registration and login
- **HTML Email Bodies** — Supports rich HTML email content

### 🌍 Multi-Language Support (i18n)

- **Arabic & English** — Full bilingual support (`ar` / `en`)
- **RTL / LTR Layout** — Automatic `dir` and `lang` attribute switching on the `<html>` element
- **URL-Based Routing** — Language prefix in URL (e.g., `/ar/login`, `/en/login`)
- **Language Guard** — `langGuard` validates the language parameter in every route
- **Auto-Redirect** — `redirectToLangGuard` sends users to their preferred language (defaults to Arabic)
- **Persistent Preference** — Language choice saved to `localStorage` across sessions
- **Custom Directive** — `langRouterLink` directive for language-aware navigation links

### 🛡️ Error Handling

- **Global Error Interceptor** — Catches all HTTP errors with smart retry logic
- **Retry with Exponential Backoff** — Auto-retries on network errors (status 0) and server overload (503/504) — up to 2 retries with 1s → 2s → 4s backoff
- **Centralized Error Handler Service** — Normalizes API errors, routes to appropriate UI feedback, and logs for debugging
- **Loading Interceptor** — Tracks in-flight requests to show/hide a global loading indicator

### 📋 Additional Features

- **Data Seeding** — Automatic creation of default roles and admin user on first run
- **CORS Configuration** — Configurable allowed origins for cross-origin requests
- **Structured Logging** — Seq integration for centralized, searchable logs
- **OpenAPI / Swagger** — Auto-generated API documentation in development mode
- **Custom Header Interceptor** — Appends custom headers (e.g., language) to all API requests

---

## 🏗️ Architecture

### Backend — Layered Architecture

```
API/
├── Presentation/        # Controllers & Program.cs (entry point)
├── Application/         # Services, DTOs, Interfaces, Common (ServiceResult)
├── Domain/              # Entities & Constants
├── Infrastructure/      # Data context, Seeders
├── Middleware/           # DI registration, Identity config, Extensions
├── Migrations/          # EF Core database migrations
└── Assets/              # Static assets
```

### Frontend — Feature-Based Structure

```
Client/src/app/
├── core/                # Singleton services, guards, interceptors, directives, interfaces
│   ├── guards/          # authGuard, langGuard, redirectToLangGuard
│   ├── interceptors/    # error, loading, auth, header interceptors
│   ├── services/        # language, token, loading, error-handler services
│   ├── directives/      # langRouterLink directive
│   └── interfaces/      # API & language interfaces
├── components/          # Reusable UI components (navbar, menu)
├── pages/               # Feature pages
│   ├── auth/            # Login & Register pages
│   ├── main-page/       # Dashboard / home
│   ├── user/            # User management
│   └── role/            # Role management
└── shared/              # Shared components, pipes, validators, services
```

---

## ⚙️ Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v20+)
- [SQL Server](https://www.microsoft.com/sql-server)
- [Angular CLI](https://angular.dev/) (`npm install -g @angular/cli`)

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/abd-el-rahman-mustafa/Rent-Management.git
cd Rent-Management
```

### 2. Backend Setup

```bash
cd API

# Update the connection string in appsettings.json if needed
# Update EmailSettings and JwtSettings in appsettings.json

dotnet restore
dotnet ef database update
dotnet run
```

The API will start at `https://localhost:5001` (or the configured port).

### 3. Frontend Setup

```bash
cd Client

npm install
ng serve
```

The client will be available at `http://localhost:4200`.

---

## 🔑 Default Admin Account

On first run, the database is seeded with:

| Field        | Value                 |
| ------------ | --------------------- |
| **Username** | `admin`               |
| **Email**    | `aamus2024@gmail.com` |
| **Password** | `Admin@1234`          |
| **Role**     | `Admin`               |

> ⚠️ **Change the default password immediately in production!**

---

## 📝 Environment Configuration

Key settings in `API/appsettings.json`:

| Section             | Key Settings                                   |
| ------------------- | ---------------------------------------------- |
| `ConnectionStrings` | SQL Server connection string                   |
| `EmailSettings`     | SMTP host, port, sender email, credentials     |
| `JwtSettings`       | Secret key, expiry (minutes), issuer, audience |
| `AllowedOrigins`    | CORS allowed origins                           |
