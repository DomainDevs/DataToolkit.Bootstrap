using Application.Features.Supplier.Services;
using Infrastructure.Common.Diagnostics;
using Serilog;

try
{

    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Iniciando Servicio...");

    // ---------------------------------------------------------------------
    // Configuration
    // ---------------------------------------------------------------------
    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();


    //Boostrap
    builder.Services.AddBootstrap(
        true,
        (typeof(Application.AssemblyReference).Assembly, "Application.Features", "Services")
        //(typeof(Persistence.AssemblyReference).Assembly, "Persistence", "Repositories")
        );

    //Manual
    builder.Services.AddSingleton<IProviderService, ProviderService>();

    // ---------------------------------------------------------------------
    // Build
    // ---------------------------------------------------------------------
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    // ---------------------------------------------------------------------
    // Endpoints
    // ---------------------------------------------------------------------
    app.MapControllers();

    Log.Information("Servicio iniciando correctamente...");
    app.Run();
}
catch (Exception ex) when (!ex.GetType()
    .Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Log.Information(">> Servicio finalizado, error critico!! <<");
    StartupDiagnostics.LogStartupError(ex);
}
finally
{
    Log.Information(">> Servicio finalizado y recursos liberados <<");
    Log.CloseAndFlush();
}