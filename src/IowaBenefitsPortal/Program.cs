using System.Text.Json;
using System.Text.Json.Serialization;
using IowaBenefitsPortal.Middleware;
using IowaBenefitsPortal.Repositories;
using IowaBenefitsPortal.Services;

// Partial class so WebApplicationFactory in the test project can host this app.
public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // JSON: camelCase to match the front-end fetch code; enums as strings.
        builder.Services.AddControllers().AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // CORS is deliberately NOT enabled: the demo front end is served by
        // this same app, so cross-origin access is not needed.
        builder.Services.AddSingleton<EligibilityService>();
        builder.Services.AddSingleton<ApplicationService>();

        // Default store is in-memory (no database needed to run the demo).
        // To use SQL Server instead, set ConnectionStrings:IowaBenefits
        // (environment variable, never in source) and swap this registration
        // for SqlServerApplicationRepository — see docs/SQL_SERVER.md.
        var sqlConnection = builder.Configuration.GetConnectionString("IowaBenefits");
        if (!string.IsNullOrWhiteSpace(sqlConnection))
        {
            builder.Services.AddSingleton<IApplicationRepository>(_ => new SqlServerApplicationRepository(sqlConnection));
        }
        else
        {
            builder.Services.AddSingleton<IApplicationRepository, InMemoryApplicationRepository>();
        }

        var app = builder.Build();

        // Middleware order matters: exception handling first so it catches
        // everything below it, then security headers, then request logging.
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        app.UseMiddleware<SecurityHeadersMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseDefaultFiles(); // Serves wwwroot/index.html at "/".
        app.UseStaticFiles();
        app.MapControllers();
        app.Run();
    }
}
