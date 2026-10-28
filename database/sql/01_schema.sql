-- 01_schema.sql — SQL Server schema for the demo portal.
-- Run in a database you control; no production data, no real citizens.
-- Fictional demo data only.
CREATE TABLE dbo.Services
(
    ServiceId     NVARCHAR(50)  NOT NULL PRIMARY KEY,
    ServiceName   NVARCHAR(200) NOT NULL,
    Summary       NVARCHAR(1000) NOT NULL
);
GO

CREATE TABLE dbo.Applications
(
    ReferenceNumber   NVARCHAR(30)  NOT NULL PRIMARY KEY,
    FirstName         NVARCHAR(60)  NOT NULL,
    LastName          NVARCHAR(60)  NOT NULL,
    City              NVARCHAR(80)  NOT NULL,
    ZipCode           CHAR(5)       NOT NULL CHECK (ZipCode LIKE '[0-9][0-9][0-9][0-9][0-9]'),
    HouseholdSize     INT           NOT NULL CHECK (HouseholdSize BETWEEN 1 AND 20),
    MonthlyIncome     DECIMAL(12,2) NOT NULL CHECK (MonthlyIncome >= 0),
    EligibilityOutcome NVARCHAR(30) NOT NULL,
    Status            NVARCHAR(30)  NOT NULL DEFAULT 'Received',
    SubmittedAtUtc    DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE INDEX IX_Applications_SubmittedAtUtc ON dbo.Applications (SubmittedAtUtc DESC);
GO

CREATE TABLE dbo.ApplicationServices
(
    ReferenceNumber NVARCHAR(30) NOT NULL,
    ServiceId       NVARCHAR(50) NOT NULL,
    PRIMARY KEY (ReferenceNumber, ServiceId),
    FOREIGN KEY (ReferenceNumber) REFERENCES dbo.Applications (ReferenceNumber),
    FOREIGN KEY (ServiceId) REFERENCES dbo.Services (ServiceId)
);
GO
