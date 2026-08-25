using SciFor.Api.Adapters;
using SciFor.Api.Dtos;
using SciFor.Domain.Ports;
using SciFor.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "O";
});

builder.Services.AddProblemDetails();

builder.Services.AddSingleton<ILinearGrid, LinearGridService>();
builder.Services.AddSingleton<ILogarithmicGrid, LogarithmicGridService>();
builder.Services.AddSingleton<IFiniteDifference, FiniteDifferenceService>();
builder.Services.AddSingleton<IFermiDirac, FermiDiracService>();

builder.Services.AddSingleton<LinearGridHttpAdapter>();
builder.Services.AddSingleton<LogarithmicGridHttpAdapter>();
builder.Services.AddSingleton<FiniteDifferenceHttpAdapter>();
builder.Services.AddSingleton<FermiDiracHttpAdapter>();

var app = builder.Build();

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
            .CreateLogger("SciFor.Api.ExceptionHandler");
        logger.LogError("Unhandled exception correlationId={CorrelationId}", context.TraceIdentifier);
        await Results.Problem(
                detail: "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error")
            .ExecuteAsync(context);
    });
});

app.MapPost("/v1/linspace", (LinspaceRequest request, LinearGridHttpAdapter adapter, HttpContext http) =>
    adapter.Handle(request, http));

app.MapPost("/v1/logspace", (LogspaceRequest request, LogarithmicGridHttpAdapter adapter, HttpContext http) =>
    adapter.Handle(request, http));

app.MapPost("/v1/deriv", (DerivRequest request, FiniteDifferenceHttpAdapter adapter, HttpContext http) =>
    adapter.Handle(request, http));

app.MapPost("/v1/fermi", (FermiRequest request, FermiDiracHttpAdapter adapter, HttpContext http) =>
    adapter.Handle(request, http));

app.Run();

public partial class Program;
