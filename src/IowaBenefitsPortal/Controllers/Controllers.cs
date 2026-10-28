using System.ComponentModel.DataAnnotations;
using IowaBenefitsPortal.Dtos;
using IowaBenefitsPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace IowaBenefitsPortal.Controllers;

/// <summary>API contract, versioned by route prefix (v1). See README for the full contract.</summary>
[ApiController]
[Route("api/v1/[controller]")]
[Route("api/[controller]")]
public sealed class ServicesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult List() => Ok(ServiceCatalog.All.Select(s => new
    {
        s.Id,
        s.Name,
        s.Summary,
        s.Audience,
    }));

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(string id)
    {
        var service = ServiceCatalog.Find(id);
        return service is null
            ? NotFound(new { error = "Service not found." })
            : Ok(new { service.Id, service.Name, service.Summary, service.Audience });
    }
}

[ApiController]
[Route("api/v1/[controller]")]
[Route("api/[controller]")]
public sealed class ApplicationsController(ApplicationService applications) : ControllerBase
{
    // POST /api/v1/applications — invalid bodies return 400 with field errors
    // via [ApiController] model-state handling; business-rule failures below
    // return 400 with a plain errors list, keeping one shape for clients.
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateApplicationRequest request, CancellationToken ct)
    {
        var (application, errors) = await applications.CreateAsync(request, ct);
        if (application is null)
        {
            return BadRequest(new { errors });
        }

        var response = ApplicationService.ToResponse(application);
        return CreatedAtAction(nameof(Get), new { reference = response.Reference }, response);
    }

    [HttpGet("{reference}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string reference, CancellationToken ct)
    {
        var application = await applications.GetAsync(reference, ct);
        return application is null
            ? NotFound(new { error = "No application was found for that reference number." })
            : Ok(ApplicationService.ToResponse(application));
    }
}

[ApiController]
[Route("health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "ok", demo = true, timestampUtc = DateTimeOffset.UtcNow });
}
