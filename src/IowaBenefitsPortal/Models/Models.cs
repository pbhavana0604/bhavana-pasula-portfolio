namespace IowaBenefitsPortal.Models;

/// <summary>
/// A fictional public service offered through the demo portal.
/// Every description carries the "fictional" label so a demo
/// listing is never mistaken for a real programme rule.
/// </summary>
public sealed class ServiceOffering
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Summary { get; init; }
    public required string Audience { get; init; }
    /// <summary>Monthly income threshold for the fictional demo rule, by household size (1..8+).</summary>
    public required IReadOnlyDictionary<int, decimal> MonthlyIncomeThresholds { get; init; }
}

public enum ApplicationStatus { Received, InReview, NeedsInformation, ApprovedDemo, NotEligibleDemo }

/// <summary>
/// Stored application. Only demo fields are kept; no SSN, no date of
/// birth, no documents and no payment details are ever collected.
/// </summary>
public sealed class BenefitApplication
{
    public required string Reference { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string City { get; init; }
    public required string ZipCode { get; init; }
    public required int HouseholdSize { get; init; }
    public required decimal MonthlyIncome { get; init; }
    public required IReadOnlyList<string> ServiceIds { get; init; }
    public required string EligibilityOutcome { get; init; }
    public required IReadOnlyList<string> EligibilityReasons { get; init; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Received;
    public DateTimeOffset SubmittedAtUtc { get; init; } = DateTimeOffset.UtcNow;
}
