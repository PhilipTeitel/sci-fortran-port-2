using SciFor.Api.Dtos;
using SciFor.Domain.Ports;
using SciFor.Domain.Validation;

namespace SciFor.Api.Adapters;

public sealed class LogarithmicGridHttpAdapter
{
    private readonly ILogarithmicGrid _port;
    private readonly ILogger<LogarithmicGridHttpAdapter> _logger;

    public LogarithmicGridHttpAdapter(ILogarithmicGrid port, ILogger<LogarithmicGridHttpAdapter> logger)
    {
        _port = port;
        _logger = logger;
    }

    public IResult Handle(LogspaceRequest request, HttpContext httpContext)
    {
        var correlationId = httpContext.TraceIdentifier;
        _logger.LogInformation(
            "logspace request correlationId={CorrelationId} num={Num} base={Base}",
            correlationId,
            request.Num,
            request.Base);

        try
        {
            if (request.Start is null || request.Stop is null || request.Num is null)
            {
                throw new DomainValidationException("logspace: start, stop, and num are required.");
            }

            var baseValue = request.Base ?? 10.0;
            var result = _port.Compute(request.Start.Value, request.Stop.Value, request.Num.Value, baseValue);

            _logger.LogInformation(
                "logspace success correlationId={CorrelationId} length={Length}",
                correlationId,
                result.Values.Length);

            return Results.Ok(new ValuesResponse { Values = result.Values });
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(
                ex,
                "logspace validation failed correlationId={CorrelationId}",
                correlationId);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid logspace request");
        }
    }
}
