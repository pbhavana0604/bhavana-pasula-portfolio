using System.Collections.Concurrent;
using IowaBenefitsPortal.Models;
using Microsoft.Data.SqlClient;

namespace IowaBenefitsPortal.Repositories;

public interface IApplicationRepository
{
    Task SaveAsync(BenefitApplication application, CancellationToken ct = default);
    Task<BenefitApplication?> GetByReferenceAsync(string reference, CancellationToken ct = default);
}

/// <summary>
/// Default demo store: in-memory, seeded with fictional applications
/// so the status page works without a database. Swapped for SQL Server
/// in production by registering SqlServerApplicationRepository instead.
/// </summary>
public sealed class InMemoryApplicationRepository : IApplicationRepository
{
    private readonly ConcurrentDictionary<string, BenefitApplication> _store = new(StringComparer.OrdinalIgnoreCase);

    public InMemoryApplicationRepository()
    {
        Seed("IA-DEMO-2026-000101", "Alex", "Example", "Des Moines", "50309", 3, 1800,
            ["food-assistance"], "Eligible", ApplicationStatus.InReview);
        Seed("IA-DEMO-2026-000102", "Jordan", "Sample", "Cedar Rapids", "52401", 2, 2600,
            ["child-care"], "NeedsReview", ApplicationStatus.NeedsInformation);
    }

    private void Seed(string reference, string first, string last, string city, string zip,
        int size, decimal income, string[] services, string outcome, ApplicationStatus status) =>
        _store[reference] = new BenefitApplication
        {
            Reference = reference,
            FirstName = first,
            LastName = last,
            City = city,
            ZipCode = zip,
            HouseholdSize = size,
            MonthlyIncome = income,
            ServiceIds = services,
            EligibilityOutcome = outcome,
            EligibilityReasons = ["Fictional seeded demo record."],
            Status = status,
            SubmittedAtUtc = new DateTimeOffset(2026, 10, 20, 12, 0, 0, TimeSpan.Zero),
        };

    public Task SaveAsync(BenefitApplication application, CancellationToken ct = default)
    {
        _store[application.Reference] = application;
        return Task.CompletedTask;
    }

    public Task<BenefitApplication?> GetByReferenceAsync(string reference, CancellationToken ct = default) =>
        Task.FromResult(_store.TryGetValue(reference.Trim(), out var found) ? found : null);
}

/// <summary>
/// SQL Server implementation. Every value is a parameter — string
/// concatenation is never used to build SQL. The connection string
/// comes from configuration (environment variable / Key Vault), never
/// from code; see docs/SQL_SERVER.md. Not registered by default, so
/// the demo runs with no database at all.
/// </summary>
public sealed class SqlServerApplicationRepository(string connectionString) : IApplicationRepository
{
    public async Task SaveAsync(BenefitApplication application, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(ct);
        await using var tx = await connection.BeginTransactionAsync(ct);
        try
        {
            await using var cmd = new SqlCommand(
                """
                INSERT INTO dbo.Applications
                    (ReferenceNumber, FirstName, LastName, City, ZipCode, HouseholdSize,
                     MonthlyIncome, EligibilityOutcome, Status, SubmittedAtUtc)
                VALUES
                    (@ReferenceNumber, @FirstName, @LastName, @City, @ZipCode, @HouseholdSize,
                     @MonthlyIncome, @EligibilityOutcome, @Status, @SubmittedAtUtc);
                """, connection, (SqlTransaction)tx);
            cmd.Parameters.AddWithValue("@ReferenceNumber", application.Reference);
            cmd.Parameters.AddWithValue("@FirstName", application.FirstName);
            cmd.Parameters.AddWithValue("@LastName", application.LastName);
            cmd.Parameters.AddWithValue("@City", application.City);
            cmd.Parameters.AddWithValue("@ZipCode", application.ZipCode);
            cmd.Parameters.AddWithValue("@HouseholdSize", application.HouseholdSize);
            cmd.Parameters.AddWithValue("@MonthlyIncome", application.MonthlyIncome);
            cmd.Parameters.AddWithValue("@EligibilityOutcome", application.EligibilityOutcome);
            cmd.Parameters.AddWithValue("@Status", application.Status.ToString());
            cmd.Parameters.AddWithValue("@SubmittedAtUtc", application.SubmittedAtUtc.UtcDateTime);
            await cmd.ExecuteNonQueryAsync(ct);

            foreach (var serviceId in application.ServiceIds)
            {
                await using var link = new SqlCommand(
                    "INSERT INTO dbo.ApplicationServices (ReferenceNumber, ServiceId) VALUES (@ReferenceNumber, @ServiceId);",
                    connection, (SqlTransaction)tx);
                link.Parameters.AddWithValue("@ReferenceNumber", application.Reference);
                link.Parameters.AddWithValue("@ServiceId", serviceId);
                await link.ExecuteNonQueryAsync(ct);
            }

            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<BenefitApplication?> GetByReferenceAsync(string reference, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(ct);
        await using var cmd = new SqlCommand("EXEC dbo.usp_GetApplicationByReference @ReferenceNumber;", connection);
        cmd.Parameters.AddWithValue("@ReferenceNumber", reference.Trim());
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct))
        {
            return null;
        }

        return new BenefitApplication
        {
            Reference = reader.GetString(reader.GetOrdinal("ReferenceNumber")),
            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
            LastName = reader.GetString(reader.GetOrdinal("LastName")),
            City = reader.GetString(reader.GetOrdinal("City")),
            ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
            HouseholdSize = reader.GetInt32(reader.GetOrdinal("HouseholdSize")),
            MonthlyIncome = reader.GetDecimal(reader.GetOrdinal("MonthlyIncome")),
            ServiceIds = [],
            EligibilityOutcome = reader.GetString(reader.GetOrdinal("EligibilityOutcome")),
            EligibilityReasons = ["Retrieved from SQL Server."],
            Status = Enum.Parse<ApplicationStatus>(reader.GetString(reader.GetOrdinal("Status"))),
            SubmittedAtUtc = reader.GetDateTime(reader.GetOrdinal("SubmittedAtUtc")),
        };
    }
}
