using IowaBenefitsPortal.Dtos;
using IowaBenefitsPortal.Models;
using IowaBenefitsPortal.Repositories;

namespace IowaBenefitsPortal.Services;

/// <summary>
/// Fictional catalogue. Thresholds are invented for the demo and are
/// NOT real Iowa programme limits. The rule is kept in one place so a
/// reviewer can read it, test it, and change it without touching the API.
/// </summary>
public static class ServiceCatalog
{
    // Household-size keys: 1..8, where 8 means "8 or more".
    private static IReadOnlyDictionary<int, decimal> Scale(decimal baseAmount, decimal step) =>
        Enumerable.Range(1, 8).ToDictionary(n => n, n => baseAmount + step * (n - 1));

    public static readonly IReadOnlyList<ServiceOffering> All =
    [
        new()
        {
            Id = "child-care",
            Name = "Child Care Assistance (fictional demo)",
            Summary = "Fictional example of help paying for child care while a parent works or studies. Not a real programme or eligibility rule.",
            Audience = "Households with children in paid child care",
            MonthlyIncomeThresholds = Scale(2400, 550),
        },
        new()
        {
            Id = "food-assistance",
            Name = "Food Assistance (fictional demo)",
            Summary = "Fictional example of help buying food. Modelled on the shape of a food-assistance programme; limits here are invented for the demo.",
            Audience = "Households needing help with food costs",
            MonthlyIncomeThresholds = Scale(2100, 450),
        },
        new()
        {
            Id = "health-coverage",
            Name = "Health Coverage (fictional demo)",
            Summary = "Fictional example of health-coverage help. Not Medicaid, not a real eligibility determination.",
            Audience = "Households needing help with health coverage costs",
            MonthlyIncomeThresholds = Scale(2800, 600),
        },
        new()
        {
            Id = "housing-assistance",
            Name = "Housing Assistance (fictional demo)",
            Summary = "Fictional example of help with housing costs. No real waiting list, voucher, or determination is involved.",
            Audience = "Households needing help with rent or housing costs",
            MonthlyIncomeThresholds = Scale(2000, 400),
        },
    ];

    public static ServiceOffering? Find(string id) =>
        All.FirstOrDefault(s => string.Equals(s.Id, id, StringComparison.OrdinalIgnoreCase));
}

/// <summary>
/// Transparent demo eligibility rule, deliberately simple:
/// under the threshold for every selected service -&gt; Eligible;
/// within 15% above the lowest threshold -&gt; LikelyEligible;
/// otherwise -&gt; NeedsReview. It never makes a real determination.
/// </summary>
public sealed class EligibilityService
{
    public EligibilityResult Evaluate(int householdSize, decimal monthlyIncome, IEnumerable<string> serviceIds)
    {
        var sizeKey = Math.Min(Math.Max(householdSize, 1), 8);
        var thresholds = serviceIds
            .Select(ServiceCatalog.Find)
            .Where(s => s is not null)
            .Select(s => s!.MonthlyIncomeThresholds[sizeKey])
            .ToList();

        if (thresholds.Count == 0)
        {
            return new EligibilityResult
            {
                Outcome = "NeedsReview",
                Reasons = ["No recognised service was selected, so no demo check could be run."],
            };
        }

        var lowest = thresholds.Min();
        if (monthlyIncome <= lowest)
        {
            return new EligibilityResult
            {
                Outcome = "Eligible",
                Reasons = [$"Demo check: monthly income is at or below the lowest fictional threshold for the selected services (${lowest:N0} for a household of {householdSize})."],
            };
        }

        if (monthlyIncome <= lowest * 1.15m)
        {
            return new EligibilityResult
            {
                Outcome = "LikelyEligible",
                Reasons = [$"Demo check: monthly income is within 15% above the lowest fictional threshold (${lowest:N0} for a household of {householdSize}); a real review would consider more factors."],
            };
        }

        return new EligibilityResult
        {
            Outcome = "NeedsReview",
            Reasons = [$"Demo check: monthly income is above the lowest fictional threshold (${lowest:N0} for a household of {householdSize}). A real determination would need a full application and verification."],
        };
    }
}

/// <summary>Coordinates validation, eligibility, reference numbering and storage.</summary>
public sealed class ApplicationService(
    IApplicationRepository repository,
    EligibilityService eligibility,
    ILogger<ApplicationService> logger)
{
    private static int _sequence = 200; // Seeded demo refs are 101-102; new refs start at 201 so they never overwrite a seed.

    public async Task<(BenefitApplication? Application, IReadOnlyList<string> Errors)> CreateAsync(
        CreateApplicationRequest request, CancellationToken ct = default)
    {
        var errors = new List<string>();

        // Business rules beyond DataAnnotations: unknown service ids, duplicates.
        var known = request.ServiceIds.Where(id => ServiceCatalog.Find(id) is not null).ToList();
        if (known.Count != request.ServiceIds.Count)
        {
            errors.Add("One or more selected services were not recognised.");
        }
        if (request.ServiceIds.Distinct(StringComparer.OrdinalIgnoreCase).Count() != request.ServiceIds.Count)
        {
            errors.Add("Each service may only be selected once.");
        }
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var result = eligibility.Evaluate(request.HouseholdSize, request.MonthlyIncome, known);
        var reference = $"IA-DEMO-2026-{Interlocked.Increment(ref _sequence):D6}";

        var application = new BenefitApplication
        {
            Reference = reference,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            City = request.City.Trim(),
            ZipCode = request.ZipCode,
            HouseholdSize = request.HouseholdSize,
            MonthlyIncome = request.MonthlyIncome,
            ServiceIds = known,
            EligibilityOutcome = result.Outcome,
            EligibilityReasons = result.Reasons,
            Status = ApplicationStatus.Received,
        };

        await repository.SaveAsync(application, ct);
        // Log the reference and outcome only — never names, income or ZIP in logs.
        logger.LogInformation("Demo application {Reference} stored with outcome {Outcome}", reference, result.Outcome);
        return (application, []);
    }

    public Task<BenefitApplication?> GetAsync(string reference, CancellationToken ct = default) =>
        repository.GetByReferenceAsync(reference, ct);

    public static ApplicationResponse ToResponse(BenefitApplication a) => new()
    {
        Reference = a.Reference,
        Status = a.Status.ToString(),
        EligibilityOutcome = a.EligibilityOutcome,
        EligibilityReasons = a.EligibilityReasons,
        ServiceIds = a.ServiceIds,
        City = a.City,
        ZipCode = a.ZipCode,
        HouseholdSize = a.HouseholdSize,
        SubmittedAtUtc = a.SubmittedAtUtc,
    };
}
