-- 02_seed_fictional.sql — fictional people only. "Alex Example" and
-- "Jordan Sample" are invented names; no real person is represented.
INSERT INTO dbo.Services (ServiceId, ServiceName, Summary) VALUES
    ('child-care', 'Child Care Assistance (fictional demo)', 'Fictional example of help paying for child care.'),
    ('food-assistance', 'Food Assistance (fictional demo)', 'Fictional example of help buying food.'),
    ('health-coverage', 'Health Coverage (fictional demo)', 'Fictional example of health-coverage help.'),
    ('housing-assistance', 'Housing Assistance (fictional demo)', 'Fictional example of help with housing costs.');
GO

INSERT INTO dbo.Applications
    (ReferenceNumber, FirstName, LastName, City, ZipCode, HouseholdSize, MonthlyIncome, EligibilityOutcome, Status, SubmittedAtUtc)
VALUES
    ('IA-DEMO-2026-000101', 'Alex', 'Example', 'Des Moines', '50309', 3, 1800.00, 'Eligible', 'InReview', '2026-10-20T12:00:00Z'),
    ('IA-DEMO-2026-000102', 'Jordan', 'Sample', 'Cedar Rapids', '52401', 2, 2600.00, 'NeedsReview', 'NeedsInformation', '2026-10-20T12:00:00Z');
GO

INSERT INTO dbo.ApplicationServices (ReferenceNumber, ServiceId) VALUES
    ('IA-DEMO-2026-000101', 'food-assistance'),
    ('IA-DEMO-2026-000102', 'child-care');
GO
