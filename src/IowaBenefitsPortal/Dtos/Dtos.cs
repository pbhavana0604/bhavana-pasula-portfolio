using System.ComponentModel.DataAnnotations;

namespace IowaBenefitsPortal.Dtos;

/// <summary>
/// Inbound application. DataAnnotations give the first, declarative
/// validation pass; <see cref="Services.ApplicationService"/> applies
/// the business rules (known service ids, consent) on top.
/// </summary>
public sealed class CreateApplicationRequest
{
    [Required(ErrorMessage = "Enter your first name.")]
    [StringLength(60, ErrorMessage = "First name must be 60 characters or fewer.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter your last name.")]
    [StringLength(60, ErrorMessage = "Last name must be 60 characters or fewer.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter your city or town.")]
    [StringLength(80, ErrorMessage = "City must be 80 characters or fewer.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a 5-digit ZIP code.")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "ZIP code must be exactly 5 digits.")]
    public string ZipCode { get; set; } = string.Empty;

    [Range(1, 20, ErrorMessage = "Household size must be between 1 and 20.")]
    public int HouseholdSize { get; set; }

    [Range(0, 1_000_000, ErrorMessage = "Monthly income must be 0 or more.")]
    public decimal MonthlyIncome { get; set; }

    [MinLength(1, ErrorMessage = "Select at least one service.")]
    public List<string> ServiceIds { get; set; } = [];

    /// <summary>Must be true: the demo stores only the fields shown on the form.</summary>
    [Range(typeof(bool), "true", "true", ErrorMessage = "Confirm that the information you entered is for this demonstration only.")]
    public bool Consent { get; set; }
}

public sealed class ApplicationResponse
{
    public required string Reference { get; init; }
    public required string Status { get; init; }
    public required string EligibilityOutcome { get; init; }
    public required IReadOnlyList<string> EligibilityReasons { get; init; }
    public required IReadOnlyList<string> ServiceIds { get; init; }
    public required string City { get; init; }
    public required string ZipCode { get; init; }
    public required int HouseholdSize { get; init; }
    public required DateTimeOffset SubmittedAtUtc { get; init; }
}

public sealed class EligibilityResult
{
    public required string Outcome { get; init; } // Eligible | LikelyEligible | NeedsReview
    public required IReadOnlyList<string> Reasons { get; init; }
}
