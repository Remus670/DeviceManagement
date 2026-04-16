USE DeviceManagementDb;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'Remus Catalin')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('Remus Catalin', 'Backend Developer', 'Cluj');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'Remus Sabadus')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('Remus Sabadus', 'QA Engineer', 'Cluj');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'Andrei Chiorean')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('Andrei Chiorean', 'Frontend Developer', 'The Office');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'David Man')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('David Man', 'Project Manager', 'The Office');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'Alex Pop')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('Cristian Andrei', 'Business Analyst', 'The Office');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'Mihai Ionescu')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('Maxim Ionut', 'Mobile Tester', 'The Office');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'Samsung S20 FE')
BEGIN
    INSERT INTO dbo.Devices
    (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmount, Description, UserId)
    VALUES
    ('Samsung S20 FE', 'Samsung', 'Phone', 'Android', '13', 'Exynos 990', '6 GB',
     'Balanced Samsung phone suitable for everyday use and internal tasks', 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'Samsung S22 Ultra')
BEGIN
    INSERT INTO dbo.Devices
    (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmount, Description, UserId)
    VALUES
    ('Samsung S22 Ultra', 'Samsung', 'Phone', 'Android', '14', 'Snapdragon 8 Gen 1', '12 GB',
     'High-end Samsung smartphone used for development and performance testing', 2);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'iPhone 17')
BEGIN
    INSERT INTO dbo.Devices
    (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmount, Description, UserId)
    VALUES
    ('iPhone 17', 'Apple', 'Phone', 'iOS', '18', 'A19 Bionic', '8 GB',
     'Premium Apple smartphone with excellent camera, good for photography and daily use', 3);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'Pixel 8')
BEGIN
    INSERT INTO dbo.Devices
    (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmount, Description, UserId)
    VALUES
    ('Pixel 8', 'Google', 'Phone', 'Android', '14', 'Google Tensor G3', '8 GB',
     'Budget-friendly Google phone for basic usage and light testing', 4);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'iPad Air')
BEGIN
    INSERT INTO dbo.Devices
    (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmount, Description, UserId)
    VALUES
    ('iPad Air', 'Apple', 'Tablet', 'iPadOS', '17', 'M2', '8 GB',
     'Tablet useful for presentations, note-taking and meeting support', 5);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'OnePlus 12')
BEGIN
    INSERT INTO dbo.Devices
    (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmount, Description, UserId)
    VALUES
    ('OnePlus 12', 'OnePlus', 'Phone', 'Android', '14', 'Snapdragon 8 Gen 3', '12 GB',
     'Phone reserved for conference meetings and mobile app testing', 6);
END
GO
