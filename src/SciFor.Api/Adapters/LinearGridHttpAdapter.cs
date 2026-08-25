using SciFor.Api.Dtos;
using SciFor.Domain.Ports;
using SciFor.Domain.Validation;

namespace SciFor.Api.Adapters;

public sealed class LinearGridHttpAdapter
{
    private readonly ILinearGrid _port;
    private readonly ILogger<LinearGridHttpAdapter> _logger;

    public LinearGridHttpAdapter(ILinearGrid port, ILogger<LinearGridHttpAdapter> logger)
    {
        _port = port;
        _logger = logger;
    }

    public IResult Handle(LinspaceRequest request, HttpContext httpContext)
    {
        var correlationId = httpContext.TraceIdentifier;
        _logger.LogInformation(
            "linspace request correlationId={CorrelationId} num={Num} mesh={Mesh}",
            correlationId,
            request.Num,
            request.Mesh);

        try
        {
            if (request.Start is null || request.Stop is null || request.Num is null)
            {
                throw new DomainValidationException("linspace: start, stop, and num are required.");
            }

            var includeStart = request.Istart ?? true;
            var includeStop = request.Iend ?? true;
            var returnSpacing = request.Mesh == true;

            var result = _port.Compute(
                request.Start.Value,
                request.Stop.Value,
                request.Num.Value,
                includeStart,
                includeStop,
                returnSpacing);

            _logger.LogInformation(
                "linspace success correlationId={CorrelationId} length={Length}",
                correlationId,
                result.Values.Length);

            return Results.Ok(new LinspaceResponse
            {
                Values = result.Values,
                Mesh = result.Spacing,
            });
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(
                ex,
                "linspace validation failed correlationId={CorrelationId}",
                correlationId);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid linspace request");
        }
    }
}
