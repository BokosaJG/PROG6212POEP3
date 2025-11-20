-- Schema for AspNetUsers
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] NVARCHAR(450) NOT NULL PRIMARY KEY,
    [UserName] NVARCHAR(256) NULL,
    [NormalizedUserName] NVARCHAR(256) NULL,
    [Email] NVARCHAR(256) NULL,
    [NormalizedEmail] NVARCHAR(256) NULL,
    [EmailConfirmed] BIT NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NULL,
    [SecurityStamp] NVARCHAR(MAX) NULL,
    [ConcurrencyStamp] NVARCHAR(MAX) NULL,
    [PhoneNumber] NVARCHAR(MAX) NULL,
    [PhoneNumberConfirmed] BIT NOT NULL,
    [TwoFactorEnabled] BIT NOT NULL,
    [LockoutEnd] DATETIMEOFFSET NULL,
    [LockoutEnabled] BIT NOT NULL,
    [AccessFailedCount] INT NOT NULL,
    [FullName] NVARCHAR(MAX) NULL,
    [Role] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL
);

-- Data for AspNetUsers
INSERT INTO [dbo].[AspNetUsers] (
    Id, UserName, NormalizedUserName, Email, NormalizedEmail,
    EmailConfirmed, AccessFailedCount, ConcurrencyStamp,
    SecurityStamp, TwoFactorEnabled, LockoutEnabled,
    PhoneNumberConfirmed, FullName, Role, CreatedAt
)
VALUES
('1','lecturer@cmcs.com','LECTURER@CMCS.COM','lecturer@cmcs.com','LECTURER@CMCS.COM',1,0,'c3fd08ee-20bf-4e4b-843b-476958c4196e',NULL,0,0,0,'John Lecturer','Lecturer','2025-11-18T20:40:00.000Z'),
('2','coordinator@cmcs.com','COORDINATOR@CMCS.COM','coordinator@cmcs.com','COORDINATOR@CMCS.COM',1,0,'afd060ff-2e26-4759-b3ad-e0f834d62654',NULL,0,0,0,'Jane Coordinator','Coordinator','2025-11-18T20:40:00.000Z'),
('3','manager@cmcs.com','MANAGER@CMCS.COM','manager@cmcs.com','MANAGER@CMCS.COM',1,0,'894e8d97-e76d-4fb6-a939-0ea7b43005c9',NULL,0,0,0,'Bob Manager','Manager','2025-11-18T20:40:00.000Z');
