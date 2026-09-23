
 # 💌 E-Greetings

### Digital Greeting Card Platform — ASP.NET Core MVC

---

## 👨‍💻 Project Information

| | |
|---|---|
| **Project Name** | E-Greetings |
| **Project Type** | Web Application (ASP.NET Core MVC) |
| **Project Category** | E-Greeting / Digital Card Platform (early-stage / scaffold) |
| **Developer** | Not specified in project files |
| **GitHub Repository** | Not found in the uploaded project |
| **Frontend** | Razor Views (`.cshtml`) + Bootstrap 5 + Font Awesome + jQuery |
| **Backend** | ASP.NET Core MVC (.NET 8, C#) |
| **Database** | Microsoft SQL Server, via Entity Framework Core (configured, but not yet populated with a working schema — see [Section 5](#5-database-implementation)) |
| **API** | REST API was not found / not verified — this is a traditional server-rendered MVC app with no API controllers |
| **Authentication** | Not implemented — no login/registration logic, no password hashing, no session/cookie authentication middleware |
| **Architecture** | ASP.NET Core MVC with Controllers, Razor Views, an EF Core `DbContext`, and two standalone helper services (Email, OTP) that are defined but not yet wired into any user-facing flow |
| **Admin Panel** | Not implemented — an "Admin Login" link exists in the UI but points to a non-existent static page |
| **User/Customer Platform** | A public marketing homepage and Login/Register pages exist, but are UI-only (see below) |

---

## 1. Project Overview

E-Greetings is an ASP.NET Core MVC (.NET 8) web application scaffolded for a digital greeting-card platform, where users would eventually register, browse card templates by occasion, customize cards, and send them. In its current state, the project contains a working ASP.NET Core MVC skeleton — routing, a database context, a `Users` entity, email/OTP helper services, and Bootstrap-based Razor views for a homepage, login page, and registration page — but **the core user-facing features (account creation, login, browsing real categories, and sending cards) are not yet functionally wired together**. The homepage and category navigation are built from a static Bootstrap HTML template with placeholder images and hard-coded links to `.html` files that do not exist anywhere in the project, and the Login/Register forms do not submit to any controller action.

**Main purpose (as scaffolded):** A platform for sending digital greeting cards for occasions such as birthdays, weddings, New Year, and festivals.

**Main users (intended, not yet implemented):** Registered "User" accounts and an "Admin" role (a `Role` field exists on the `Users` entity, defaulting to `"User"`), though no registration, login, or role-based logic currently exists.

**Main sections present in code:** Home (marketing landing page), Auth (Login/Register pages), and a Category controller with a single, currently broken action (see [Section 11](#11-admin-panel) and [Section 12](#12-module-by-module-implementation)).

**Overall architecture:** A single ASP.NET Core MVC project (`E-Greetings.csproj`, target framework `net8.0`) using Entity Framework Core with a SQL Server connection string, three controllers (`HomeController`, `AuthController`, `CategoryController`), a `Users` entity/DbContext, DTOs and a custom validation attribute for registration, and two backend services (`EmailService`, `OtpService`) registered for dependency injection but not currently invoked from any controller action.

---

## 2. Project Category

**E-Greeting / Digital Card Platform — early-stage scaffold.** Structurally this is a standard full-stack MVC web application, but functionally it is closer to a **UI/architecture scaffold**: the visual design and some backend building blocks exist, but the actual account, authentication, and card-sending features have not yet been connected end-to-end.

---

## 3. Technologies Used

### Frontend
- Razor Views (`.cshtml`) rendered server-side by ASP.NET Core MVC
- Bootstrap 5 (bundled locally under `wwwroot/lib/bootstrap`)
- Font Awesome 6 (loaded via CDN in `_Layout.cshtml`)
- jQuery (bundled locally under `wwwroot/lib/jquery`)
- jQuery Validation + jQuery Validation Unobtrusive (bundled under `wwwroot/lib/`, referenced via `_ValidationScriptsPartial.cshtml`, but this partial is not actually included in the Login/Register views — see [Section 20](#20-error-handling--validation))

### Backend
- **ASP.NET Core MVC**, targeting **.NET 8** (`<TargetFramework>net8.0</TargetFramework>`)
- **Entity Framework Core 8** (`Microsoft.EntityFrameworkCore.SqlServer` 8.0.22, `Microsoft.EntityFrameworkCore.Tools` 8.0.22) — the only two NuGet packages referenced in the project

### Database
- **Microsoft SQL Server**, configured via a `DefaultConnection` connection string in `appsettings.json` and consumed through `ApplicationDbContext` (Entity Framework Core, `UseSqlServer`).
- **REST API was not found / not verified** — no `[ApiController]` classes or API route attributes exist anywhere in the project; all controllers are standard MVC `Controller` classes returning Razor views.

### Libraries & Frameworks actually referenced in code
- `System.Net.Mail.SmtpClient` (from the .NET base class library) — used directly in `EmailService` for sending email via SMTP, no third-party mail package.
- No ORM beyond EF Core; no logging framework beyond the built-in `ILogger<T>`; no third-party validation, mapping, or DI libraries.

### UI / Design
- The homepage, login, and register pages are all built from what is clearly a **purchased or downloaded Bootstrap admin/landing template**, using placeholder imagery from `placehold.co` and static, non-functional links (e.g., `categories/birthday.html`, `card-editor.html`, `dashboard/user-dashboard.html`, `admin-login.html`) — **none of these referenced `.html` files exist anywhere in the project**, confirming this is template/mockup content rather than a working front-end.

---

## 4. Project Structure

```
E-Greetings-main/
├── Controllers/
│   ├── HomeController.cs            # Index (homepage) + generic Error action
│   ├── AuthController.cs            # Login/Register — GET-only, view-rendering only
│   └── CategoryController.cs        # Categories action — no matching view exists (see Section 11)
│
├── Core/
│   ├── Services/
│   │   ├── EmailService.cs          # SMTP email sender (SmtpClient), reads EmailSettings
│   │   └── OtpService.cs            # Generates a 6-digit OTP + 5-minute expiry, sends it via EmailService
│   └── Validations/
│       └── UniqueEmailAttribute.cs  # Custom [UniqueEmail] validation attribute, checks Users table
│
├── Data/
│   └── ApplicationDbContext.cs      # EF Core DbContext — single DbSet<Users>
│
├── Models/
│   ├── DTOs/
│   │   ├── RegisterDTO.cs           # Name/Email/Password/ConfirmPassword with data-annotation validation
│   │   └── EmailDTO.cs              # To/Subject/Body for EmailService
│   ├── Entities/
│   │   └── Users.cs                 # UserId (Guid), Name, Email, PasswordHash, Role, CreatedAt, IsActive
│   ├── EmailSettings.cs             # Host/Port/EnableSSL/Username/Password (bound from configuration)
│   └── ErrorViewModel.cs            # Standard ASP.NET Core error view model
│
├── Views/
│   ├── Home/
│   │   └── Index.cshtml             # Marketing homepage (hero, categories, featured templates, CTA)
│   ├── Auth/
│   │   ├── Login.cshtml             # Static-style login form (Layout = null), no server-side binding
│   │   └── Register.cshtml          # Static-style register form (Layout = null), no server-side binding
│   ├── Shared/
│   │   ├── _Layout.cshtml           # Site-wide navbar + footer (used by Home/Index only)
│   │   ├── _Layout.cshtml.css       # Scoped layout styles
│   │   ├── _ValidationScriptsPartial.cshtml
│   │   └── Error.cshtml             # Standard ASP.NET Core error page
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml            # Sets default Layout = "_Layout"
│
├── Properties/
│   └── launchSettings.json          # Local dev URLs/profiles (http/https/IIS Express)
│
├── wwwroot/
│   ├── css/site.css
│   ├── js/site.js
│   └── lib/                         # Bootstrap, jQuery, jQuery Validation (client-side libraries only)
│
├── Program.cs                       # App startup: DbContext, EmailSettings, EmailService, OtpService, MVC routing
├── E-Greetings.csproj                # .NET 8 project file (2 NuGet packages)
├── appsettings.json                  # Connection string + Email SMTP settings
└── appsettings.Development.json      # Development-only logging overrides
```

*(The `bin/` and `obj/` build-output folders were excluded from this structure as they are compiler-generated artifacts, not source.)*

### Approximate File Counts (source only, excluding `bin/`/`obj/`)
| File Type | Count |
|---|---|
| C# (`.cs`) | 9 |
| Razor views (`.cshtml`) | 9 |
| JSON config (`appsettings*.json`, `launchSettings.json`) | 3 |
| CSS (custom, excluding bundled libraries) | 2 (`site.css`, `_Layout.cshtml.css`) |
| JavaScript (custom, excluding bundled libraries) | 1 (`site.js`) |
| Third-party front-end libraries (`wwwroot/lib/`) | Bootstrap, jQuery, jQuery Validation, jQuery Validation Unobtrusive (vendor code, not authored in this project) |

No SQL files, no EF Core Migrations folder, and no image/media assets (the homepage uses external `placehold.co` placeholder URLs, not local images) were found in the project.

---

## 5. Database Implementation

- **Database technology:** Microsoft SQL Server, accessed through **Entity Framework Core 8** (`UseSqlServer`).
- **Connection configuration:** `appsettings.json` → `ConnectionStrings:DefaultConnection`, pointing to a local SQL Server instance using Windows/Trusted authentication (`Trusted_Connection=True`).
- **Database name (as configured):** `E-Greetings`.
- **DbContext:** `Data/ApplicationDbContext.cs`, registered in `Program.cs` via `AddDbContext<ApplicationDbContext>`.
- **Tables/entities confirmed in code:** a single `DbSet<Users>` — there is **only one table** (`Users`) defined anywhere in this project.
  - `Users` fields: `UserId` (Guid, primary key), `Name`, `Email`, `PasswordHash`, `Role` (defaults to `"User"`), `CreatedAt`, `IsActive`.
  - Two navigation properties (`SentCards`, `Subscriptions`) are **commented out** in `Users.cs` and reference entity types that **do not exist anywhere in the project** — these represent planned-but-unbuilt relationships, not implemented functionality.
- **The uploaded project does not contain a SQL database export**, and **no Entity Framework Core Migrations folder was found**, meaning the database schema has not yet been generated from this codebase — running `dotnet ef database update` against this project as-is would fail until an initial migration is created.
- **CRUD operations:** ⚠️ **No CRUD operations against the `Users` table (or any table) are implemented anywhere in the controllers.** The only code that touches the `Users` `DbSet` is `UniqueEmailAttribute`, which performs a **read-only existence check** (`db.Users.Any(u => u.Email == value)`) during model validation — and even this is never triggered, because `RegisterDTO` (which carries the `[UniqueEmail]` attribute) is never bound or validated in `AuthController`.

---

## 6. Requirements

- **.NET 8 SDK**
- **A C# IDE** such as Visual Studio 2022 (17.8+) or Visual Studio Code with the C# extension
- **Microsoft SQL Server** (LocalDB, Express, or a full instance) reachable at the host configured in `appsettings.json`, or your own connection string
- **Entity Framework Core CLI tools** (`dotnet-ef`), if you intend to create and apply migrations, since none currently exist in the project
- A modern web browser

---

## 7. Installation & Setup

1. **Extract/clone** the project to a local directory.
2. **Open** `E-Greetings.slnx` (or `E-Greetings.csproj`) in Visual Studio 2022, or use the .NET CLI from the project root.
3. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```
4. **Configure the database connection** in `appsettings.json` (`ConnectionStrings:DefaultConnection`) to point at your own SQL Server instance — the committed connection string targets a specific local machine (`desktop-fas3baq`) and will not work on another machine as-is.
5. **Configure email settings** in `appsettings.json` (`EmailSettings`) with your own SMTP credentials (see [Section 9](#9-configuration) — the repository currently contains a **live-looking Gmail address and app password committed in plain text**, which should be replaced and the original credentials rotated/revoked immediately).
6. **Create an initial EF Core migration** (none exists in the project yet):
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   *(Requires the EF Core CLI tool: `dotnet tool install --global dotnet-ef` if not already installed.)*
7. **Build and run the application:**
   ```bash
   dotnet run
   ```
   or launch via Visual Studio using the `https` or `http` profile defined in `Properties/launchSettings.json`.
8. **Open the app** in your browser (see [Section 33](#33-project-urls) for the configured local URLs).

---

## 8. Database Setup

No SQL dump file is included, and **no EF Core Migrations folder exists in the uploaded project**. To stand up the database:

1. Ensure the `DefaultConnection` string in `appsettings.json` points to a reachable SQL Server instance.
2. Generate an initial migration from the current model (`dotnet ef migrations add InitialCreate`) and apply it (`dotnet ef database update`) — this will create a database named `E-Greetings` (per the current connection string) containing a single `Users` table with the columns described in [Section 5](#5-database-implementation).
3. No seed data of any kind is defined anywhere in the project.

---

## 9. Configuration

**Configuration files found:**
- `appsettings.json` — contains `ConnectionStrings:DefaultConnection` and an `EmailSettings` section (`Host`, `Port`, `EnableSSL`, `Username`, `Password`).
- `appsettings.Development.json` — logging-level overrides only.
- `Properties/launchSettings.json` — local development URLs and the `ASPNETCORE_ENVIRONMENT` variable.

⚠️ **Security note — action required:** The uploaded `appsettings.json` contains a **real-looking SMTP username (a Gmail address) and a Gmail App Password committed in plain text** in the repository, along with a SQL Server connection string that uses Windows Trusted Authentication. In line with safe handling of credentials, this README does not reproduce those values. You should:
- Treat the committed email credential as **compromised** and revoke/rotate it immediately in your Google Account security settings.
- Move both the connection string and `EmailSettings` values out of `appsettings.json` and into a secrets mechanism such as the **.NET Secret Manager** (`dotnet user-secrets`) for local development, or environment variables / a key vault for deployment.
- Add `appsettings.json` (or at least the sensitive sections) to `.gitignore` going forward, or use `appsettings.json` only for non-sensitive defaults with an `appsettings.Local.json`/environment-variable override for secrets.

---

## 10. User / Customer Features

| Feature | Status |
|---|---|
| Marketing homepage (hero, "Browse by Occasion" section, "Featured Templates" section, call-to-action) | 🖥️ UI ONLY — static Bootstrap template content with placeholder images (`placehold.co`) and hard-coded example stats (e.g., "100+ Templates"); not backed by any real template or category data |
| Category browsing (Birthday, Wedding, New Year, Festivals) | ❌ NOT IMPLEMENTED — homepage and navbar links point to static files like `categories/birthday.html`, which **do not exist anywhere in the project**; the one `CategoryController` action that does exist has no corresponding view (see [Section 11](#11-admin-panel)) |
| Card customization / card editor | ❌ NOT IMPLEMENTED — the homepage links to `card-editor.html`, which does not exist anywhere in the project; no card entity, editor logic, or related controller exists |
| User registration | 🖥️ UI ONLY — `Register.cshtml` renders a Bootstrap form with client-side HTML5 `required`/`minlength` validation and a small inline JavaScript password-match check, but the `<form>` has **no `method="post"`, no server-side model binding, and its `action` attribute points to a non-existent static file (`dashboard/user-dashboard.html`)**; `AuthController.Register()` only returns the empty view (`GET` only — no `[HttpPost]` handler exists) |
| User login | 🖥️ UI ONLY — same pattern as Register: `Login.cshtml`'s form has no working submission target, and `AuthController.Login()` only returns the empty view |
| "Remember me" / "Forgot Password?" | 🖥️ UI ONLY — checkbox and link are present in `Login.cshtml` but have no backing logic |
| Admin Login | ❌ NOT IMPLEMENTED — a link to `admin-login.html` exists in `Login.cshtml`, but no such page, controller, or admin authentication logic exists anywhere in the project |
| Email sending (e.g., for OTP/notifications) | ⚠️ PARTIALLY IMPLEMENTED — `EmailService` contains working SMTP-sending code, and `OtpService` can generate and email a 6-digit OTP, but **neither service is called from any controller**, so this capability is not currently reachable by an end user, and the generated OTP is never persisted anywhere for later verification |
| Subscribe / Feedback pages | ❌ NOT IMPLEMENTED — navbar links to `subscribe.html` and `feedback.html`, neither of which exists anywhere in the project |
| Account uniqueness check (`[UniqueEmail]`) | ⚠️ PARTIALLY IMPLEMENTED — the validation attribute is correctly written and queries the `Users` table, but it is attached only to `RegisterDTO`, which is **never used as an action parameter anywhere**, so this check currently never executes |

---

## 11. Admin Panel

**Not implemented.** There is no dedicated admin controller, no admin views, and no role-based authorization anywhere in the project. The only admin-adjacent artifact is:
- An **"Admin Login"** link in `Login.cshtml` pointing to `admin-login.html`, which does not exist.
- A `Role` field on the `Users` entity (default `"User"`) that is never read or checked by any authorization logic.

Separately, `CategoryController.Categories()` returns `View()` with no explicit view name, which by ASP.NET Core MVC convention would look for `Views/Category/Categories.cshtml`. **No such file exists in the project** (confirmed — only `Views/Home/`, `Views/Auth/`, and `Views/Shared/` contain any `.cshtml` files), so invoking this action at runtime would currently result in an `InvalidOperationException` ("The view 'Categories' was not found").

---

## 12. Module-by-Module Implementation

### Home / Marketing Page
- **Purpose:** Public landing page introducing the platform.
- **Files:** `Controllers/HomeController.cs`, `Views/Home/Index.cshtml`, `Views/Shared/_Layout.cshtml`.
- **Frontend:** Fully built Bootstrap 5 layout (hero section, occasion cards, featured template cards, CTA banner) using placeholder images from `placehold.co` and static example copy.
- **Backend:** `HomeController.Index()` simply returns the view — no data is passed from a controller or database; all "content" (template names, counts) is hard-coded directly in the Razor markup.
- **Status:** 🖥️ UI Only — visually complete, functionally static.

### Authentication (Login/Register)
- **Purpose:** Intended to let a user create an account and sign in.
- **Files:** `Controllers/AuthController.cs`, `Views/Auth/Login.cshtml`, `Views/Auth/Register.cshtml`, `Models/DTOs/RegisterDTO.cs`, `Core/Validations/UniqueEmailAttribute.cs`.
- **Frontend implementation:** Both views are self-contained HTML pages (`Layout = null`) styled with Bootstrap, using Bootstrap's client-side `needs-validation` pattern and HTML5 `required` attributes. Neither form has server-targeting attributes (no `asp-controller`/`asp-action`, no `method="post"`, no `name` attributes matching `RegisterDTO`), and both forms' `action` attributes point to a static file that does not exist in the project.
- **Backend implementation:** `AuthController` exposes only `Register()` and `Login()` as parameterless `GET` actions that return their respective views. **There are no `[HttpPost]` actions, no password hashing calls, no `SignInAsync`/cookie or session creation, and `Program.cs` never registers an authentication scheme** (only `app.UseAuthorization()` is called — there is no corresponding `app.UseAuthentication()` or `AddAuthentication(...)` in `Program.cs`).
- **Database implementation:** The `Users` table exists via EF Core, and `RegisterDTO` is fully validated with data annotations (`[Required]`, `[EmailAddress]`, `[Compare]`, and the custom `[UniqueEmail]`), but **since no controller action accepts a `RegisterDTO` parameter, none of this validation logic is ever invoked**, and no code anywhere actually inserts a row into the `Users` table.
- **Status:** 🖥️ UI Only for both Login and Register. The supporting data model and validation attribute are implemented in isolation but not connected to any endpoint.

### Category Browsing
- **Purpose:** Intended to let users browse greeting card templates by occasion.
- **Files:** `Controllers/CategoryController.cs`.
- **Frontend:** No corresponding view file exists.
- **Backend:** A single `Categories()` action exists and returns `View()`, but with no accompanying `Views/Category/Categories.cshtml`, so this action is currently broken (see [Section 11](#11-admin-panel)). All actual category "browsing" experienced by a user comes from static, non-existent `.html` links on the homepage and navbar.
- **Status:** ❌ Not implemented (broken action, no view, no data model for categories or templates).

### Email Service
- **Purpose:** Send transactional emails via SMTP.
- **File:** `Core/Services/EmailService.cs`.
- **Implementation:** A straightforward wrapper around `System.Net.Mail.SmtpClient`, configured from the `EmailSettings` options bound in `Program.cs`. Builds and sends an HTML email given an `EmailDTO` (To/Subject/Body).
- **Status:** ✅ Implemented and functionally complete **as a standalone service**, but ⚠️ it is registered in dependency injection and never actually called from any controller, so it is not currently reachable through the application's UI.

### OTP Service
- **Purpose:** Generate a one-time password and email it to a user (e.g., for verification).
- **File:** `Core/Services/OtpService.cs`.
- **Implementation:** Generates a random 6-digit numeric code with a 5-minute expiry timestamp, formats it into an `EmailDTO`, and delegates sending to `EmailService`.
- **Important limitation:** The generated OTP and its expiry are **only held in a local `OtpModel` instance returned by the method call — they are not persisted to the database, cache, or session**, so there is currently no mechanism anywhere in the codebase to later verify a user-submitted OTP against the one that was generated and sent.
- **Status:** ⚠️ Partially implemented (generation and sending work in isolation) and, like `EmailService`, ❌ not currently invoked from any controller or user-facing flow.

### Unique Email Validation
- **Purpose:** Prevent duplicate account registration by email.
- **File:** `Core/Validations/UniqueEmailAttribute.cs`.
- **Implementation:** A custom `ValidationAttribute` that resolves `ApplicationDbContext` from the validation context and checks for an existing user with the same email.
- **Status:** ⚠️ Correctly implemented in isolation, but effectively dormant since `RegisterDTO` (the only place this attribute is applied) is never used as a bound parameter anywhere in the project.

---

## 13. API Implementation

**REST API was not found / not verified.** This project contains no `[ApiController]`-attributed classes, no `[Route("api/...")]` attributes, and no JSON-returning endpoints of any kind. All three controllers (`HomeController`, `AuthController`, `CategoryController`) are conventional MVC controllers that return `ViewResult`s (Razor HTML views), routed through the single default MVC route registered in `Program.cs`:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

| Controller | Action | HTTP Method | Returns |
|---|---|---|---|
| `Home` | `Index` | GET (default) | `Views/Home/Index.cshtml` |
| `Home` | `Error` | GET (default) | `Views/Shared/Error.cshtml` |
| `Auth` | `Register` | GET (default) | `Views/Auth/Register.cshtml` |
| `Auth` | `Login` | GET (default) | `Views/Auth/Login.cshtml` |
| `Auth` | `Error` | GET (default) | `Views/Shared/Error.cshtml` |
| `Category` | `Categories` | GET (default) | ❌ View not found at runtime (no matching `.cshtml`) |
| `Category` | `Error` | GET (default) | `Views/Shared/Error.cshtml` |

No action in the project accepts `[HttpPost]`, form data, or a request body — every action shown above is a bare, parameterless `GET` that simply renders a view.

---

## 14. Authentication & Authorization

- **Registration:** ❌ Not implemented — UI only, no backend handler (see [Section 12](#12-module-by-module-implementation)).
- **Login:** ❌ Not implemented — UI only, no backend handler.
- **Logout:** ❌ Not implemented — no such action or link exists anywhere in the project.
- **Sessions:** ❌ Not implemented — `Program.cs` does not configure `AddSession`, distributed cache, or any session middleware.
- **JWT:** ❌ Not implemented — no JWT packages, token issuance, or bearer authentication configured anywhere.
- **Password hashing:** ❌ Not implemented — the `Users` entity has a `PasswordHash` column, but **no code anywhere in the project actually hashes a password or writes a value to this column**; no hashing library (e.g., `BCrypt`, ASP.NET Core Identity's `PasswordHasher`) is referenced in `E-Greetings.csproj`.
- **Password reset:** ❌ Not implemented.
- **Email verification:** ❌ Not implemented (the OTP service exists but is unreachable and doesn't persist its code — see [Section 12](#12-module-by-module-implementation)).
- **Role-based access:** ❌ Not implemented — the `Role` field exists on `Users` but is never checked by any `[Authorize]` attribute or custom authorization logic (in fact, **no controller or action in this project has an `[Authorize]` attribute at all**).
- **Admin authentication:** ❌ Not implemented.
- **Protected routes:** ❌ Not implemented — `Program.cs` calls `app.UseAuthorization()` but never `app.UseAuthentication()` or registers an authentication scheme (e.g., cookies), so there is currently no mechanism by which a request could even be recognized as "authenticated" in the first place.

**UI vs. backend distinction:** The Login and Register pages are visually complete and use client-side (HTML5/Bootstrap) validation only. There is no working authentication backend behind either page in this project as uploaded.

---

## 15. CRUD Operations

| Module | Create | Read | Update | Delete | Status |
|---|---|---|---|---|---|
| Users | ❌ | ⚠️ (read-only existence check in `[UniqueEmail]`, never actually invoked) | ❌ | ❌ | No functional CRUD exists |
| Categories/Templates | ❌ | ❌ | ❌ | ❌ | No data model, no CRUD — homepage content is hard-coded markup |
| Cards (sending/customizing) | ❌ | ❌ | ❌ | ❌ | No entity, controller, or view exists for this concept anywhere in the project |

No table in this project currently has a single functional Create, Update, or Delete operation implemented in any controller.

---

## 16. Frontend Implementation

- **Pages:** 3 controllers → 3 functioning views (`Home/Index`, `Auth/Login`, `Auth/Register`) plus a non-functional `Category/Categories` action with no view, and the shared `Error` view.
- **Components:** No partial views or reusable Razor components exist beyond `_Layout.cshtml`, `_ValidationScriptsPartial.cshtml`, and `_ViewImports`/`_ViewStart`.
- **Layout:** `_Layout.cshtml` defines a shared navbar and footer and is used (by default, via `_ViewStart.cshtml`) for every view **except** `Login.cshtml` and `Register.cshtml`, which explicitly opt out (`Layout = null`) and are fully self-contained HTML documents with their own `<head>`/`<body>`.
- **Navigation:** The navbar's Home/Login/Register links correctly use ASP.NET Core tag helpers (`asp-controller`/`asp-action`) and will route properly. All other navbar and homepage links (Birthday, Wedding, New Year, Festivals, Subscribe, Feedback, and every "customize"/"view all" link) are plain `href="....html"` attributes pointing to files that do not exist in this project.
- **Forms:** Present on Register/Login with HTML5 `required` validation and one small inline script (password-match check on Register); neither form is wired for server submission.
- **Responsive design:** Bootstrap 5's grid and responsive utility classes are used throughout (`col-md-*`, `navbar-expand-lg`, etc.), so the existing pages are responsive by virtue of the Bootstrap framework, though this has not been manually verified across devices.
- **CSS framework:** Bootstrap 5 (local copy in `wwwroot/lib/bootstrap`), plus a small custom `site.css`.
- **JavaScript:** jQuery (bundled), a default `site.js` (created by the ASP.NET Core project template, not custom application logic), and one inline script for password-match validation on the Register page.
- **Charts/animations:** None found.
- **Assets:** No local image assets — all imagery on the homepage is loaded from the external `placehold.co` placeholder-image service.

---

## 17. Backend Implementation

- **Framework/language:** ASP.NET Core MVC, C#, targeting .NET 8.
- **Routes:** A single conventional route (`{controller=Home}/{action=Index}/{id?}`) registered in `Program.cs`; no attribute routing is used anywhere.
- **Controllers:** `HomeController`, `AuthController`, `CategoryController` — all thin, with no business logic beyond returning a view; each also exposes an identical boilerplate `Error()` action (this pattern is duplicated three times rather than centralized).
- **Services:** `EmailService` (SMTP sending) and `OtpService` (OTP generation + email dispatch) are defined and registered for dependency injection (`AddTransient<EmailService>`, `AddScoped<OtpService>`) but are not consumed by any controller.
- **Middleware:** `Program.cs` configures `UseExceptionHandler` (non-development only), `UseHsts` (non-development only), `UseHttpsRedirection`, `UseStaticFiles`, `UseRouting`, and `UseAuthorization` — notably **without** a matching `UseAuthentication()` call or any registered authentication scheme.
- **Database connection:** Standard EF Core `DbContext` injection via `AddDbContext<ApplicationDbContext>`, using the SQL Server provider.
- **Validation:** Data-annotation validation is defined on `RegisterDTO` (including the custom `[UniqueEmail]` attribute), but since no controller action currently binds to `RegisterDTO`, this validation never executes at runtime.
- **Error handling:** The framework-provided `/Home/Error` exception handler path is configured for non-development environments; each controller also has its own duplicate `Error()` action, which is redundant but not broken.

---

## 18. Responsive Design

- **Desktop/Tablet/Mobile:** The completed pages (`Home/Index`, `Login`, `Register`) use Bootstrap 5's responsive grid system (`col-lg-*`, `col-md-*`, `col-6`, `navbar-expand-lg` with a collapsible mobile menu), which provides baseline responsiveness typical of Bootstrap-based sites.
- **Responsive navigation:** ✅ The navbar includes a `navbar-toggler`/`collapse` pattern for mobile screens.
- **Responsive forms:** ✅ The Login/Register forms use Bootstrap's standard responsive form controls.
- **Responsive tables/cards:** The homepage's occasion and template sections use responsive Bootstrap card/column classes (`col-md-3 col-6`, `col-md-4`) that reflow at different breakpoints.

No custom, project-specific responsive engineering was found beyond what Bootstrap provides out of the box.

---

## 19. Security Inspection

| Practice | Status |
|---|---|
| Password hashing | ❌ Not implemented — no hashing code exists anywhere, and no user data is ever written to the database in the first place |
| Authentication middleware | ❌ Not configured (`UseAuthentication()` is never called in `Program.cs`) |
| Authorization / role checks | ❌ Not implemented — no `[Authorize]` attributes anywhere |
| SQL injection protection | ✅ N/A risk-wise — the only database access uses EF Core's parameterized LINQ queries (`db.Users.Any(u => u.Email == value)`); no raw SQL is constructed anywhere |
| Input validation | ⚠️ Defined (via data annotations on `RegisterDTO`) but never actually exercised, since the DTO is unused |
| File upload validation | Not applicable — no file upload functionality exists in this project |
| XSS protection | ✅ Razor's default output encoding applies to the one view (`Home/Index`) that renders any dynamic-looking content, though in practice all current homepage content is static markup, not user-supplied data |
| CSRF protection | ❌ Not applicable yet — no forms in this project currently POST to the server, so ASP.NET Core's anti-forgery token system (`@Html.AntiForgeryToken()` / `[ValidateAntiForgeryToken]`) is not yet in use anywhere, and will need to be added once real form submission is implemented |
| Environment variables / secret storage | ❌ Not used — SMTP credentials and the database connection string are committed in plain text in `appsettings.json` (see the security note in [Section 9](#9-configuration)) |
| HTTPS/HSTS | ✅ Configured — `UseHttpsRedirection()` and `UseHsts()` (non-development) are present in `Program.cs`, consistent with the default ASP.NET Core MVC template |

⚠️ **Most significant finding:** A real SMTP account (Gmail address + app password) is committed directly in `appsettings.json`. This should be treated as an active credential leak — see [Section 9](#9-configuration) for recommended remediation.

---

## 20. Error Handling & Validation

- **Client-side validation:** Register.cshtml has HTML5 `required`/`minlength` attributes plus one inline JavaScript function that flags mismatched passwords via `setCustomValidity`. Login.cshtml has HTML5 `required` attributes only.
- **jQuery Unobtrusive Validation:** The `_ValidationScriptsPartial.cshtml` partial (which would enable ASP.NET Core's server-driven unobtrusive validation, tied to data-annotation attributes like those on `RegisterDTO`) exists in `Views/Shared/` but **is not `@await Html.PartialAsync(...)`'d or `<partial>`'d into either `Login.cshtml` or `Register.cshtml`**, so it currently has no effect on those pages.
- **Server-side validation:** Data annotations exist on `RegisterDTO` (`[Required]`, `[EmailAddress]`, `[Compare]`, `[UniqueEmail]`), but since no controller action binds to this DTO, `ModelState` validation for registration is never actually triggered.
- **Database errors:** No explicit try/catch or error-handling logic exists around any EF Core or SMTP operation (e.g., `EmailService.SendEmailAsync` does not catch `SmtpException`).
- **HTTP/global error handling:** The standard ASP.NET Core MVC `/Home/Error` exception-handler page is configured for non-development environments, using the framework's default `ErrorViewModel`/`Error.cshtml`.
- **Empty states:** Not applicable — no list/data-driven views exist yet in this project.

---

## 21. Search, Filtering & Sorting

**Not implemented.** No search input, filter control, sort option, or corresponding backend query exists anywhere in the project. The homepage's "Browse by Occasion" and "Featured Templates" sections are static, hard-coded Bootstrap card markup, not the result of any search, filter, or database query.

---

## 22. Reports & Analytics

**Not applicable.** This project contains no dashboard, chart, report, or analytics feature of any kind, hardcoded or otherwise.

---

## 23. Testing Checklist

```
[ ] dotnet restore completes successfully
[ ] Database connection string points to a reachable SQL Server instance
[ ] An initial EF Core migration has been created and applied (none exists yet)
[ ] dotnet run starts the application without errors
[ ] Home page (/) loads and renders the marketing content
[ ] /Auth/Login renders the login form
[ ] /Auth/Register renders the registration form
[ ] /Category/Categories currently throws a "view not found" error (expected, given no view exists)
[ ] No automated test project exists in the uploaded solution
```

**Existing automated tests:** None found — there is no test project (e.g., `xUnit`/`NUnit`/`MSTest`) anywhere in the uploaded solution.

---

## 24. Confirmed Implemented Features

- ✅ ASP.NET Core MVC project skeleton (.NET 8) with working routing, DI, and configuration binding
- ✅ EF Core `DbContext` correctly configured against SQL Server, with a `Users` entity/table definition
- ✅ Fully designed, responsive Bootstrap 5 homepage (static content)
- ✅ Fully designed Login and Register page layouts with client-side HTML5 validation
- ✅ Working, self-contained `EmailService` (SMTP sending via `SmtpClient`) — functional in isolation
- ✅ Working `OtpService` (OTP generation + email dispatch) — functional in isolation
- ✅ `RegisterDTO` with complete data-annotation validation, including a custom `[UniqueEmail]` attribute that correctly queries the database
- ✅ Standard ASP.NET Core error-handling page and HTTPS/HSTS configuration

## 25. Partially Implemented Features

- ⚠️ **Email/OTP services** — the code to generate and send an OTP email is fully functional as a standalone service, but it is never called from any controller, and even if called, the OTP is never persisted anywhere for later verification.
- ⚠️ **Registration validation** — `RegisterDTO` and `[UniqueEmail]` are correctly implemented but never actually exercised, since no controller action uses `RegisterDTO` as a parameter.
- ⚠️ **Client-side form validation** — HTML5 attributes and one inline script exist, but the project's own jQuery Unobtrusive Validation partial is not included in the relevant views.

## 26. Features Not Implemented / Not Found

- ❌ User registration (no backend handler, no database write)
- ❌ User login / logout / sessions / cookies / JWT
- ❌ Password hashing
- ❌ Role-based access control / `[Authorize]` usage
- ❌ Admin login / admin panel of any kind
- ❌ Category browsing backend (the one existing `CategoryController` action has no matching view)
- ❌ Card creation, customization, editor, or sending
- ❌ Subscription/pricing (`subscribe.html`) and feedback (`feedback.html`) pages
- ❌ REST API of any kind
- ❌ EF Core Migrations (none exist in the project)
- ❌ Automated tests
- ❌ Environment-variable/secret-based configuration (credentials are committed in plain text)

---

## 27. Current Project Status

```
Frontend:        PARTIAL (homepage/login/register pages designed; most links/actions are non-functional)
Backend:         PARTIAL (MVC skeleton, DbContext, and two isolated services exist; not connected together)
Database:        PARTIAL (schema modeled in code; no migrations, no seed data, no CRUD wired up)
Authentication:  NOT FOUND
Admin:           NOT FOUND
API:             NOT FOUND
Categories/Cards: NOT FOUND
Security:        NEEDS IMPROVEMENT (committed SMTP credentials; no authentication/authorization configured yet)
```

---

## 28. Project Workflow

*(Based strictly on what is actually wired together in the current code — this is intentionally short, since most flows are not yet connected.)*

```
Visitor
   ↓
Home Page (static marketing content)
   ↓
Click "Login" or "Register" (routes correctly via asp-controller/asp-action)
   ↓
Login / Register page renders
   ↓
[Form submission is not wired to any backend action — the flow ends here]
```

The `EmailService`/`OtpService` and the `Users` table exist as isolated building blocks with no workflow currently connecting them to the pages above.

---

## 29. Project Architecture

```
┌─────────────────────────────┐
│   Razor Views (.cshtml)      │  Home / Auth(Login, Register) / Shared
│   Bootstrap 5 + jQuery        │
└──────────────┬─────────────────┘
               │ (Home & nav routing only — Login/Register forms not wired)
┌──────────────▼─────────────────┐
│  ASP.NET Core MVC Controllers  │  HomeController / AuthController / CategoryController
└──────────────┬─────────────────┘
               │ EF Core (DbContext)
┌──────────────▼─────────────────┐
│  SQL Server ("E-Greetings" DB)  │  Table: Users (only)
└─────────────────────────────────┘

  (Separately, registered in DI but not called by any controller:)
  EmailService  ──uses──►  OtpService
       │
       ▼
  SMTP (Gmail, per appsettings.json)
```

---

## 30. Common Errors & Solutions

| Error | Likely Cause | Solution |
|---|---|---|
| `InvalidOperationException: The view 'Categories' was not found` | `CategoryController.Categories()` has no matching `Views/Category/Categories.cshtml` | Create the missing view, or remove/rework the action until the Category feature is actually built |
| `SqlException: A network-related or instance-specific error...` on startup/first DB access | The committed connection string targets a specific developer machine (`desktop-fas3baq`) | Update `ConnectionStrings:DefaultConnection` in `appsettings.json` to your own SQL Server instance |
| `No migrations were found` / `dotnet ef database update` fails | No EF Core Migrations exist in the project | Run `dotnet ef migrations add InitialCreate` before `dotnet ef database update` |
| `SmtpException: Authentication failed` if `EmailService` is later wired up and called | Gmail requires an App Password (not the account password) for SMTP, and the committed one may since have been revoked | Generate a new Gmail App Password (or switch SMTP provider) and store it via user secrets/environment variables, not in `appsettings.json` |
| Login/Register forms "do nothing" when clicked | No `[HttpPost]` action exists to receive the submission, and the form's `action` attribute points to a non-existent static file | This is expected given the current state of the code — implement a POST-handling action bound to `RegisterDTO`/a new `LoginDTO` before the forms can function |
| `dotnet run` starts but every non-Home/Auth link 404s | Category, Subscribe, Feedback, and Admin pages/routes do not exist | Expected in the current codebase — these features have not yet been built |

---

## 31. Future Improvements

*(Reasonable next steps based on the gaps identified above — not existing functionality.)*

- Implement `[HttpPost]` actions for `AuthController.Register`/`Login` that bind to `RegisterDTO` (and a new `LoginDTO`), validate `ModelState`, hash passwords (e.g., with ASP.NET Core Identity's `PasswordHasher<T>` or a library like BCrypt.Net), and persist new users to the `Users` table.
- Configure an authentication scheme (e.g., cookie authentication via `AddAuthentication().AddCookie()`) and call `app.UseAuthentication()` before `app.UseAuthorization()` in `Program.cs`, so that login state can actually be established and checked.
- Add `[Authorize]`/role-based checks using the existing `Role` field once authentication exists.
- Build a real `Category`/`Template` data model and view (`Views/Category/Categories.cshtml`) instead of the current static homepage links.
- Design and implement the actual card creation/customization/sending feature referenced throughout the UI (`card-editor.html`) — currently no entity or logic for this exists at all.
- Wire `OtpService` into the registration/login flow, and **persist generated OTPs (with expiry) to the database or a cache** so they can actually be verified against a user-submitted code.
- Add CSRF protection (`@Html.AntiForgeryToken()` + `[ValidateAntiForgeryToken]`) to any forms once they start posting to the server.
- Move all secrets (SMTP credentials, connection strings) out of `appsettings.json` and into the .NET Secret Manager for local dev and environment variables/a key vault for deployment; rotate the currently-committed Gmail credential immediately.
- Generate and check in EF Core Migrations so the database schema can be reproducibly created.
- Add an automated test project (unit and/or integration tests) — none currently exists.

---

## 32. Quick Start

```bash
1. dotnet restore
2. Update ConnectionStrings:DefaultConnection and EmailSettings in appsettings.json with your own values
3. dotnet ef migrations add InitialCreate
4. dotnet ef database update
5. dotnet run
6. Open the URL shown in the console (see Section 33)
```

---

## 33. Project URLs

Per `Properties/launchSettings.json`:

| Profile | URL |
|---|---|
| `http` | `http://localhost:5000` |
| `https` | `https://localhost:7167` (with HTTP fallback at `http://localhost:5000`) |
| IIS Express | `http://localhost:64936` (HTTP), port `44316` (SSL) |

There is no separate admin URL, API URL, or database-management URL (e.g., phpMyAdmin) configured — this is a single ASP.NET Core MVC application with no split front-end/back-end hosting.

**GitHub repository:** Not found in the uploaded project.

---

## 34. Final Project Summary

E-Greetings, as uploaded, is a **well-organized ASP.NET Core MVC (.NET 8) skeleton** for a digital greeting-card platform, with a properly configured Entity Framework Core `DbContext` against SQL Server, a clean project layout (Controllers/Core/Data/Models/Views), a fully designed and responsive Bootstrap 5 marketing homepage, and two genuinely functional backend services (`EmailService` and `OtpService`) for sending email and one-time codes. However, the project's headline features — **user registration, login, category browsing, and card creation/sending** — are **not yet implemented end-to-end**: the Login and Register pages are static HTML forms with no server-side submission handling, no password hashing occurs anywhere, no authentication middleware is configured, the `CategoryController`'s one action has no corresponding view, and every category/card/subscribe/feedback link across the site points to static files that do not exist in the project. A real SMTP credential is also committed in plain text in `appsettings.json` and should be rotated immediately. In its current state, this project is best described as an early-stage UI and architecture scaffold — the visual design and some backend plumbing are in place, but the core product functionality still needs to be built and connected.

---

## 35. Final Project Information

```
PROJECT NAME:     E-Greetings
PROJECT TYPE:     Web Application (ASP.NET Core MVC, .NET 8)
CATEGORY:         E-Greeting / Digital Card Platform (early-stage scaffold)
FRONTEND:         Razor Views + Bootstrap 5 + Font Awesome + jQuery
BACKEND:          ASP.NET Core MVC (C#, .NET 8)
DATABASE:         Microsoft SQL Server via Entity Framework Core 8 (single "Users" table; no migrations yet)
API:              Not found / not verified — no REST API exists
AUTHENTICATION:   Not implemented
ARCHITECTURE:     Razor Views → MVC Controllers → EF Core DbContext → SQL Server
                  (EmailService/OtpService exist as isolated, unconnected services)
ADMIN:            Not implemented
USER/CUSTOMER:    UI-only Login/Register/homepage; no functional account or card features yet
INSTALLATION:     dotnet restore && dotnet ef database update && dotnet run
DEVELOPER:        Youza Ahsan
GITHUB:          (https://github.com/youzaahsan/E-Greetings)
```

---

<div align="center">

**E-Greetings** — ASP.NET Core MVC Digital Greeting Card Platform (early-stage)

</div>
