IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AppTrips] (
    [Id] int NOT NULL IDENTITY,
    [TpepPickupDatetime] datetime2 NOT NULL,
    [TpepDropoffDatetime] datetime2 NOT NULL,
    [PassengerCount] int NULL,
    [TripDistance] float NOT NULL,
    [StoreAndFwdFlag] nvarchar(max) NOT NULL,
    [PULocationID] int NOT NULL,
    [DOLocationID] int NOT NULL,
    [FareAmount] decimal(18,2) NOT NULL,
    [TipAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_AppTrips] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_AppTrips_PULocationID] ON [AppTrips] ([PULocationID]);
GO

CREATE INDEX [IX_AppTrips_TripDistance] ON [AppTrips] ([TripDistance]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260304101109_Init', N'8.0.0');
GO

COMMIT;
GO

