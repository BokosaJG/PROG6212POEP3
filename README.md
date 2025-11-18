Contract Monthly Claim System (CMCS)

Project Overview

The Contract Monthly Claim System (CMCS) is a .NET web-based application designed to streamline the monthly claim submission and approval workflow for Independent Contractor (IC) lecturers. It includes role-based access control, automated calculations, file management, and reporting tools.

System Features

Role-Based Access Control

HR Administrator – Full system access

Lecturers – Submit and track claims

Programme Coordinators – Verify and approve claims

Academic Managers – Final approval authority

Core Functionalities
Lecturer Features

Submit monthly claims with automatic hour/amount calculations

Upload supporting documents (PDF, DOCX, XLSX, JPG, PNG)

Real-time claim status tracking

View claim history

Automatic validation (maximum 180 hours per month)

HR Features

Full user management

Create and manage user accounts

Set lecturer hourly rates

Generate comprehensive reports

Reset passwords

No public registration — HR creates all accounts


Approval Workflow

Two-tier approval: Coordinator → Manager

Session-based authentication

Real-time status updates

Rejection with reason tracking

Transparent approval history


Reporting & Analytics


Custom date-range reports

Status-based filtering

Lecturer-specific reporting

Total amount calculations

Printable report formats


Technical Stack

Backend

ASP.NET Core 8.0

ASP.NET Core Identity

SQL Server (EF Core)

MVC Architecture


Frontend

Bootstrap 5.3

Custom CSS (responsive)

jQuery

Font Awesome

Security

Role-based authorization

Session management

CSRF protection

Input validation

Secure, validated file uploads


Project Structure
CMCS/
├─ Controllers/
│  ├─ HomeController.cs
│  ├─ HRController.cs
│  └─ AccountController.cs
├─ Models/
│  ├─ ApplicationUser.cs
│  ├─ Claim.cs
│  └─ Document.cs
├─ ViewModels/
│  ├─ ClaimViewModel.cs
│  ├─ UserViewModel.cs
│  └─ ReportViewModel.cs
├─ Views/
│  ├─ Home/
│  ├─ HR/
│  └─ Account/
|  └─Shared
├─ Data/
│  └─ ApplicationDbContext.cs
└─ wwwroot/
   ├─ css/
   └─ uploads/


Installation & Setup
Prerequisites

.NET 8.0 SDK

SQL Server (LocalDB or Express)

Visual Studio 2022 or VS Code

Steps
1. Clone the Repository
git clone <repository-url>
cd CMCS

2. Configure Database Connection

Update appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CMCS;Trusted_Connection=true;MultipleActiveResultSets=true"
}

3. Run Database Setup
4. 
dotnet ef database update


or allow auto-create on first run.

4. Run the Application
dotnet run


or press F5 in Visual Studio.

Default Login Credentials

Role	Email	Password	Access
HR	hr@cmcs.com
	HR123!	Full access
Lecturer	lecturer@cmcs.com
	Lecturer123!	Submit claims
Coordinator	coordinator@cmcs.com
	Coordinator123!	Approve claims
Manager	manager@cmcs.com
	Manager123!	Final approval

  
Configuration
File Upload Settings

Max size: 10MB

Allowed: PDF, DOCX, XLSX, JPG, PNG

Storage: wwwroot/uploads/documents/

Session Settings

Timeout: 30 minutes

Secure cookies enabled

HTTP-only cookies enabled

Validation Rules

Hours worked: 1–180

Hourly rate: R0–R1000

File types restricted


Database Schema
Key Tables

AspNetUsers: Identity users with role + rate info

Claims: Claim submissions with status tracking

Documents: Uploaded files linked to claims

Relationships

Users → Claims (1:N)

Claims → Documents (1:N)

Authorization managed via Identity roles

Security Features

ASP.NET Core Identity authentication

Role-based authorization

Secure session storage

Anti-forgery tokens

Input validation & sanitization

File type/size enforcement

Usage Guide
For Lecturers

Login

Go to Submit Claim

Enter hours (auto-calculated)

Upload supporting docs

Submit & track status

For HR

Manage users in Manage Users

Create accounts and assign roles

Set hourly rates

Generate reports

For Coordinators & Managers

View Pending Claims

Approve or reject with comments

View history and logs

Troubleshooting
Login Issues

Ensure user exists

Verify role assignment

Check password

Reporting Issues

Confirm claim data exists

Verify date filters

Check DB connection

File Upload Errors

Ensure < 10MB

Allowed file type

Directory write permissions

Debug Mode

Visit /HR/DebugDatabase (HR only) to inspect data tables.

Reporting Features

Date range reports

Status filtering

Lecturer-specific summaries

Financial totals and breakdowns


Version Control


Git-based versioning with meaningful commits:

Part 1: Planning & prototype

Part 2: Functional web application

Part 3: HR management + reporting enhancements

Support


For technical issues:

Check the debug database

Verify roles & permissions

Confirm SQL Server connectivity

Review application logs


License

© The Independent Institute of Education (Pty) Ltd 2025
