-- 03_queries.sql — parameterized examples only. Never build SQL by
-- concatenating user input; use parameters, as in SqlServerApplicationRepository.
-- Example 1: lookup by reference (parameter, never inline text).
DECLARE @ReferenceNumber NVARCHAR(30) = N'IA-DEMO-2026-000101';
SELECT ReferenceNumber, City, ZipCode, HouseholdSize, EligibilityOutcome, Status
FROM dbo.Applications
WHERE ReferenceNumber = @ReferenceNumber;
GO

-- Example 2: stored procedure used by the repository.
CREATE OR ALTER PROCEDURE dbo.usp_GetApplicationByReference
    @ReferenceNumber NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ReferenceNumber, FirstName, LastName, City, ZipCode,
           HouseholdSize, MonthlyIncome, EligibilityOutcome, Status, SubmittedAtUtc
    FROM dbo.Applications
    WHERE ReferenceNumber = @ReferenceNumber;
END;
GO

-- Example 3: eligibility view — a reporting view, not a determination.
CREATE OR ALTER VIEW dbo.vw_ApplicationSummary AS
SELECT a.ReferenceNumber, a.City, a.EligibilityOutcome, a.Status, a.SubmittedAtUtc,
       COUNT(s.ServiceId) AS ServiceCount
FROM dbo.Applications AS a
LEFT JOIN dbo.ApplicationServices AS s ON s.ReferenceNumber = a.ReferenceNumber
GROUP BY a.ReferenceNumber, a.City, a.EligibilityOutcome, a.Status, a.SubmittedAtUtc;
GO
