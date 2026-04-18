# Device Management System

A full-stack web application for managing company mobile devices, built with ASP.NET Core and Angular.

## Tech Stack

- **Backend:** C#, ASP.NET Web API, .NET 10
- **Database:** MS SQL Server
- **Frontend:** Angular 21
- **Authentication:** JWT
- **LLM:** Google Gemini
- **Version Control:** Git

## Features

### Phase 1 - Backend + API
- Full CRUD operations for devices
- MS SQL Server database with idempotent setup scripts
- RESTful API endpoints
- Integration tests for all CRUD endpoints — can be run via terminal using `dotnet test` (no SQL Server required, uses in-memory database)

### Phase 2 - User Interface
- View all devices in a grid with assigned users
- View detailed device information
- Create new devices with duplicate and field validation
- Edit existing devices
- Delete devices with confirmation modal

### Phase 3 - Authentication + Authorization
- User registration with email and password
- JWT-based authentication
- Protected routes — only logged in users can access the system
- Assign a device to yourself if unassigned
- Unassign a device previously assigned to you

### Phase 4 - AI Integration
- Implemented a Device Description Generator using **Google Gemini API** (gemini-2.5-flash model)
- When creating or editing a device, the user can click "Generate with AI" to automatically generate a professional description
- The backend constructs a prompt with the device specifications (name, manufacturer, type, OS, RAM, processor) and sends it to the Gemini REST API
- The generated description is returned to the frontend and populated in the description field
- The API key is stored securely in appsettings.json and never exposed to the frontend

### Phase 5 - Bonus: Free-Text Search
- Search devices by name, manufacturer, processor or RAM
- Case-insensitive and robust to formatting differences (extra spaces, punctuation)
- Query split into individual tokens for flexible matching
- Relevance ranking — results ordered by match strength (Name > Manufacturer > Processor > RAM)

## Prerequisites

- .NET 10 SDK (https://dotnet.microsoft.com/download)
- Node.js (https://nodejs.org/)
- Angular CLI — npm install -g @angular/cli
- MS SQL Server
- Google Gemini API key — get one free at https://aistudio.google.com/app/apikey

## How to Run Locally

### 1. Clone the repository

git clone https://github.com/Remus670/DeviceManagement.git
cd DeviceManagement

### 2. Set up the Database

Open SQL Server Management Studio and run the scripts in order:

backend/database/01_create_database.sql
backend/database/02_seed_data.sql

### 3. Configure the Backend

Open backend/appsettings.json and update with your values:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DeviceManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY"
  }
}

### 4. Run the Backend

cd backend
dotnet run

API available at http://localhost:5219
Swagger UI at http://localhost:5219/swagger

### 5. Run the Frontend

cd device-management-ui
npm install
ng serve

App available at http://localhost:4200

## Running Tests

cd backend/DeviceManagementApi.Tests
dotnet test

## Project Structure

```
DeviceManagement/
├── backend/
│   ├── Controllers/
│   │   ├── DevicesController.cs
│   │   ├── AuthController.cs
│   │   ├── AssignController.cs
│   │   ├── AiController.cs
│   │   └── SearchController.cs
│   ├── Models/
│   ├── Data/
│   ├── database/
│   │   ├── 01_create_database.sql
│   │   └── 02_seed_data.sql
│   └── DeviceManagementApi.Tests/
└── device-management-ui/
    └── src/
        └── app/
            ├── components/
            │   ├── device-list/
            │   ├── device-detail/
            │   ├── device-form/
            │   ├── login/
            │   └── register/
            ├── services/
            ├── models/
            ├── guards/
            └── interceptors/
```
## API Endpoints

Method  | Endpoint                        | Description
--------|----------------------------------|---------------------------
GET     | /api/Devices                    | Get all devices
GET     | /api/Devices/{id}               | Get device by ID
POST    | /api/Devices                    | Create device
PUT     | /api/Devices/{id}               | Update device
DELETE  | /api/Devices/{id}               | Delete device
POST    | /api/Auth/register              | Register user
POST    | /api/Auth/login                 | Login user
POST    | /api/devices/{id}/assign        | Assign device to current user
POST    | /api/devices/{id}/unassign      | Unassign device from current user
POST    | /api/Ai/generate-description    | Generate AI description
GET     | /api/devices/search?q={query}   | Search devices by relevance
