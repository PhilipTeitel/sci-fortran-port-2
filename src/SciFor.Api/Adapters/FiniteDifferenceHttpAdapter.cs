using SciFor.Api.Dtos;
using SciFor.Domain.Ports;
using SciFor.Domain.Validation;

namespace SciFor.Api.Adapters;

public sealed class FiniteDifferenceHttpAdapter
{
    private readonly IFiniteDifference _port;
    private readonly ILogger<FiniteDifferenceHttpAdapter> _logger;

    public FiniteDifferenceHttpAdapter(IFiniteDifference port, ILogger<FiniteDifferenceHttpAdapter> logger)
    {
        _port = port;
        _logger = logger;
    }

    public IResult Handle(DerivRequest request, HttpContext httpContext)
    {
        var correlationId = httpContext.TraceIdentifier;
        _logger.LogInformation(
            "deriv request correlationId={CorrelationId} length={Length}",
            correlationId,
            request.F?.Length ?? 0);

        try
        {
            if (request.F is null || request.Dh is null)
            {
                throw new DomainValidationException("deriv: f and dh are required.");
            }

            var result = _port.Compute(request.F, request.Dh.Value);

            _logger.LogInformation(
                "deriv success correlationId={CorrelationId} length={Length}",
                correlationId,
                result.Values.Length);

            return Results.Ok(new DerivResponse { Df = result.Values });
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(
                ex,
                "deriv validation failed correlationId={CorrelationId}",
                correlationId);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid deriv request");
        }
    }
}
