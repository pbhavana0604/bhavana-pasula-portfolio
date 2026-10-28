using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using IowaBenefitsPortal.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IowaBenefitsPortal.Tests;

/// <summary>
/// Unit tests for the fictional eligibility rule — the logic that is
/// easiest to get wrong and most important to pin down.
/// </summary>
public sealed class EligibilityServiceTests
{
    private readonly EligibilityService _service = new();

    [Fact]
    public void BelowThreshold_IsEligible()
    {
        var result = _service.Evaluate(3, 1000, ["food-assistance"]);
        Assert.Equal("Eligible", result.Outcome);
        Assert.NotEmpty(result.Reasons);
    }

    [Fact]
    public void JustAboveThreshold_IsLikelyEligible()
    {
        // Food assistance, household 3: threshold 2100 + 2*450 = 3000; 3100 is within 15%.
        var result = _service.Evaluate(3, 3100, ["food-assistance"]);
        Assert.Equal("LikelyEligible", result.Outcome);
    }

    [Fact]
    public void FarAboveThreshold_NeedsReview()
    {
        var result = _service.Evaluate(1, 9000, ["food-assistance"]);
        Assert.Equal("NeedsReview", result.Outcome);
    }

    [Fact]
    public void UnknownService_NeedsReview()
    {
        var result = _service.Evaluate(2, 100, ["not-a-service"]);
        Assert.Equal("NeedsReview", result.Outcome);
    }
}

/// <summary>
/// API tests through WebApplicationFactory: success, validation failure,
/// not-found and error-shape cases. These run the real middleware and
/// controllers, not mocks of them.
/// </summary>
public sealed class ApiTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiTests()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    private static object ValidPayload() => new
    {
        firstName = "Alex",
        lastName = "Example",
        city = "Des Moines",
        zipCode = "50309",
        householdSize = 3,
        monthlyIncome = 1500,
        serviceIds = new[] { "food-assistance" },
        consent = true,
    };

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetServices_ReturnsFictionalCatalogue()
    {
        var services = await _client.GetFromJsonAsync<JsonElement[]>("/api/services");
        Assert.NotNull(services);
        Assert.True(services!.Length >= 4);
    }

    [Fact]
    public async Task PostApplication_Valid_Returns201WithReference()
    {
        var response = await _client.PostAsJsonAsync("/api/applications", ValidPayload());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.StartsWith("IA-DEMO-2026-", body.GetProperty("reference").GetString());
        Assert.Equal("Eligible", body.GetProperty("eligibilityOutcome").GetString());
    }

    [Fact]
    public async Task PostApplication_InvalidZip_Returns400()
    {
        var payload = ValidPayload();
        var json = JsonSerializer.Serialize(payload).Replace("\"50309\"", "\"503\"");
        var response = await _client.PostAsync("/api/applications",
            new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostApplication_NoConsent_Returns400()
    {
        var payload = new
        {
            firstName = "Alex",
            lastName = "Example",
            city = "Des Moines",
            zipCode = "50309",
            householdSize = 3,
            monthlyIncome = 1500,
            serviceIds = new[] { "food-assistance" },
            consent = false,
        };
        var response = await _client.PostAsJsonAsync("/api/applications", payload);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostApplication_UnknownService_Returns400()
    {
        var payload = new
        {
            firstName = "Alex",
            lastName = "Example",
            city = "Des Moines",
            zipCode = "50309",
            householdSize = 3,
            monthlyIncome = 1500,
            serviceIds = new[] { "nope" },
            consent = true,
        };
        var response = await _client.PostAsJsonAsync("/api/applications", payload);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetApplication_Seeded_Returns200()
    {
        var response = await _client.GetAsync("/api/applications/IA-DEMO-2026-000101");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetApplication_Unknown_Returns404()
    {
        var response = await _client.GetAsync("/api/applications/IA-DEMO-2026-999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetApplication_ThenPostedReference_RoundTrips()
    {
        var created = await _client.PostAsJsonAsync("/api/applications", ValidPayload());
        var body = await created.Content.ReadFromJsonAsync<JsonElement>();
        var reference = body.GetProperty("reference").GetString()!;
        // Note: each test gets a fresh factory/app, but the in-memory store
        // is per-app, so the reference exists within this test's app.
        var fetched = await _client.GetAsync($"/api/applications/{reference}");
        Assert.Equal(HttpStatusCode.OK, fetched.StatusCode);
    }
}

/// <summary>
/// Accessibility smoke test for the served pages: skip link, language,
/// landmarks, labels and a single h1. This is a structural check only —
/// it is NOT a WCAG compliance claim (see docs/ACCESSIBILITY.md).
/// </summary>
public sealed class HtmlAccessibilitySmokeTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public HtmlAccessibilitySmokeTests()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/apply.html")]
    [InlineData("/status.html")]
    [InlineData("/services.html")]
    [InlineData("/design-system.html")]
    public async Task Page_HasSkipLinkLandmarksAndSingleH1(string path)
    {
        var html = await _client.GetStringAsync(path);
        Assert.Contains("lang=\"en\"", html);
        Assert.Contains("skip-link", html);
        Assert.Contains("<main", html);
        Assert.Contains("<header", html);
        Assert.Contains("<footer", html);
        Assert.Equal(1, Count(html, "<h1"));
    }

    [Fact]
    public async Task ApplyPage_EveryInputHasLabel()
    {
        var html = await _client.GetStringAsync("/apply.html");
        Assert.Contains("for=\"first-name\"", html);
        Assert.Contains("for=\"zip-code\"", html);
        Assert.Contains("for=\"consent\"", html);
        Assert.DoesNotContain("placeholder=", html); // No placeholder-only labels.
    }

    private static int Count(string text, string needle)
    {
        var count = 0;
        var index = 0;
        while ((index = text.IndexOf(needle, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += needle.Length;
        }
        return count;
    }
}
