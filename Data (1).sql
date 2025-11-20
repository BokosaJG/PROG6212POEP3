-- DATA SCRIPT ------------------------------------------------------

-- NOTE: PasswordHash and SecurityStamp are generated dynamically at runtime
-- These values are placeholders and should be overwritten during actual deployment.

INSERT INTO [AspNetUsers] (
    [Id], [FullName], [Role], [CreatedAt],
    [UserName], [NormalizedUserName], [Email], [NormalizedEmail],
    [EmailConfirmed], [PasswordHash], [SecurityStamp],
    [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed],
    [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]
) VALUES
('1', 'Admin User', 'Admin', GETDATE(),
 'admin@cmcs.com', 'ADMIN@CMCS.COM', 'admin@cmcs.com', 'ADMIN@CMCS.COM',
 1, '<hashed password>', NEWID(), NEWID(), NULL, 0, 0, NULL, 0, 0),

('2', 'HR Officer', 'HR', GETDATE(),
 'hr@cmcs.com', 'HR@CMCS.COM', 'hr@cmcs.com', 'HR@CMCS.COM',
 1, '<hashed password>', NEWID(), NEWID(), NULL, 0, 0, NULL, 0, 0),

('3', 'Claims Officer', 'Claims', GETDATE(),
 'claims@cmcs.com', 'CLAIMS@CMCS.COM', 'claims@cmcs.com', 'CLAIMS@CMCS.COM',
 1, '<hashed password>', NEWID(), NEWID(), NULL, 0, 0, NULL, 0, 0);

--------------------------------------------------------------------
