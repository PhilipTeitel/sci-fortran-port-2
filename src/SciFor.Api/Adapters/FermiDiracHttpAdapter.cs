using SciFor.Api.Dtos;
using SciFor.Domain.Ports;
using SciFor.Domain.Validation;

namespace SciFor.Api.Adapters;

public sealed class FermiDiracHttpAdapter
{
    private readonly IFermiDirac _port;
    private readonly ILogger<FermiDiracHttpAdapter> _logger;

    public FermiDiracHttpAdapter(IFermiDirac port, ILogger<FermiDiracHttpAdapter> logger)
    {
        _port = port;
        _logger = logger;
    }

    public IResult Handle(FermiRequest request, HttpContext httpContext)
    {
        var correlationId = httpContext.TraceIdentifier;
        _logger.LogInformation(
            "fermi request correlationId={CorrelationId} x={X} beta={Beta}",
            correlationId,
            request.X,
            request.Beta);

        try
        {
            if (request.X is null || request.Beta is null)
            {
                throw new DomainValidationException("fermi: x and beta are required.");
            }

            var value = _port.Evaluate(request.X.Value, request.Beta.Value);

            _logger.LogInformation(
                "fermi success correlationId={CorrelationId} value={Value}",
                correlationId,
                value);

            return Results.Ok(new FermiResponse { Value = value });
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(
                ex,
                "fermi validation failed correlationId={CorrelationId}",
                correlationId);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid fermi request");
        }
    }
}
