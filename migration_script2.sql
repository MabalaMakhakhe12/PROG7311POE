BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FarmerProducts]') AND [c].[name] = N'Quantity');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [FarmerProducts] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [FarmerProducts] ALTER COLUMN [Quantity] int NOT NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FarmerProducts]') AND [c].[name] = N'ProductionDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [FarmerProducts] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [FarmerProducts] ALTER COLUMN [ProductionDate] date NOT NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FarmerProducts]') AND [c].[name] = N'Price');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [FarmerProducts] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [FarmerProducts] ALTER COLUMN [Price] real NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240521182120_ChangePriceToFloat', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [FarmerProducts];
GO

CREATE TABLE [Categories] (
    [CategoryID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryID])
);
GO

CREATE TABLE [Contact] (
    [ContactID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Subject] nvarchar(max) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY ([ContactID])
);
GO

CREATE TABLE [Employees] (
    [EmployeeID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Surname] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [mobile] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([EmployeeID])
);
GO

CREATE TABLE [Payments] (
    [PaymentID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [CardNo] nvarchar(max) NOT NULL,
    [ExpiryDate] nvarchar(max) NOT NULL,
    [Cvv] int NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [PaymentMode] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([PaymentID])
);
GO

CREATE TABLE [Users] (
    [UserID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Surname] nvarchar(max) NOT NULL,
    [UserName] nvarchar(max) NOT NULL,
    [mobileNo] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [PostCode] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserID])
);
GO

CREATE TABLE [Farmers] (
    [FarmerID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Surname] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Contact] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [EmployeeID] int NOT NULL,
    CONSTRAINT [PK_Farmers] PRIMARY KEY ([FarmerID]),
    CONSTRAINT [FK_Farmers_Employees_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employees] ([EmployeeID]) ON DELETE CASCADE
);
GO

CREATE TABLE [Products] (
    [ProductID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CategoryID] int NOT NULL,
    [FarmerID] int NOT NULL,
    [Price] real NOT NULL,
    [Quantity] int NOT NULL,
    [ProductionDate] date NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductID]),
    CONSTRAINT [FK_Products_Categories_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [Categories] ([CategoryID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Products_Farmers_FarmerID] FOREIGN KEY ([FarmerID]) REFERENCES [Farmers] ([FarmerID]) ON DELETE CASCADE
);
GO

CREATE TABLE [Carts] (
    [CartID] int NOT NULL IDENTITY,
    [ProductID] int NOT NULL,
    [UserID] int NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY ([CartID]),
    CONSTRAINT [FK_Carts_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Carts_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID]) ON DELETE CASCADE
);
GO

CREATE TABLE [Orders] (
    [OrderDetailsID] int NOT NULL IDENTITY,
    [OrderNo] nvarchar(max) NOT NULL,
    [ProductID] int NOT NULL,
    [Quantity] int NOT NULL,
    [UserID] int NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [PaymentID] int NOT NULL,
    [OrderDate] date NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderDetailsID]),
    CONSTRAINT [FK_Orders_Payments_PaymentID] FOREIGN KEY ([PaymentID]) REFERENCES [Payments] ([PaymentID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Orders_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Orders_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Carts_ProductID] ON [Carts] ([ProductID]);
GO

CREATE INDEX [IX_Carts_UserID] ON [Carts] ([UserID]);
GO

CREATE INDEX [IX_Farmers_EmployeeID] ON [Farmers] ([EmployeeID]);
GO

CREATE INDEX [IX_Orders_PaymentID] ON [Orders] ([PaymentID]);
GO

CREATE INDEX [IX_Orders_ProductID] ON [Orders] ([ProductID]);
GO

CREATE INDEX [IX_Orders_UserID] ON [Orders] ([UserID]);
GO

CREATE INDEX [IX_Products_CategoryID] ON [Products] ([CategoryID]);
GO

CREATE INDEX [IX_Products_FarmerID] ON [Products] ([FarmerID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240522132015_SecondMigration', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'UserName');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Users] ALTER COLUMN [UserName] nvarchar(100) NOT NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Email');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Users] ALTER COLUMN [Email] nvarchar(100) NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
GO

CREATE UNIQUE INDEX [IX_Users_UserName] ON [Users] ([UserName]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240522172624_thirdMigration', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Farmers] ADD [UserName] nvarchar(max) NOT NULL DEFAULT N'';
GO

ALTER TABLE [Employees] ADD [UserName] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240525153730_fourth migration', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240525153800_Fifth Migration', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240525154941_sixth', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'CreatedDate');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Users] ALTER COLUMN [CreatedDate] nvarchar(max) NOT NULL;
GO

ALTER TABLE [Farmers] ADD [Password] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526192904_seventh', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526194826_eighth', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526220538_ninth', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526220917_tenth', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526221029_eleventh', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526221236_12th', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [TestModel] (
    [ProductID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CategoryID] int NOT NULL,
    [FarmerID] int NOT NULL,
    [Price] real NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_TestModel] PRIMARY KEY ([ProductID])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526221814_14th', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [TestModel];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526222218_16th', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526222346_17th', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Farmers] (
    [FarmerID] int NOT NULL IDENTITY,
    [UserName] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Surname] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Contact] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [EmployeeID] int NOT NULL,
    CONSTRAINT [PK_Farmers] PRIMARY KEY ([FarmerID]),
    CONSTRAINT [FK_Farmers_Employees_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employees] ([EmployeeID]) ON DELETE CASCADE
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526222612_18th', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Farmers] (
    [FarmerID] int NOT NULL IDENTITY,
    [UserName] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Surname] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Contact] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [EmployeeID] int NOT NULL,
    CONSTRAINT [PK_Farmers] PRIMARY KEY ([FarmerID]),
    CONSTRAINT [FK_Farmers_Employees_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employees] ([EmployeeID]) ON DELETE CASCADE
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240526224025_19th', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'CreatedDate');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Categories] DROP COLUMN [CreatedDate];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'ImageUrl');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Categories] DROP COLUMN [ImageUrl];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240528101457_20th', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240528113651_21st', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Farmers] ADD [ConfirmPassword] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240528115048_22nd', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Farmers]') AND [c].[name] = N'ConfirmPassword');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Farmers] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Farmers] DROP COLUMN [ConfirmPassword];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240528123433_23rd', N'8.0.5');
GO

COMMIT;
GO

