using SciFor.Domain.Models;
using SciFor.Domain.Ports;
using SciFor.Domain.Validation;

namespace SciFor.Domain.Services;

/// <summary>
/// Logarithmic grid (XP-061 / TOOLS::logspace). DEF-002 zero rewrite; DEF-003/004 → validation.
/// </summary>
public sealed class LogarithmicGridService : ILogarithmicGrid
{
    private readonly ILinearGrid _linearGrid;

    public LogarithmicGridService()
        : this(new LinearGridService())
    {
    }

    public LogarithmicGridService(ILinearGrid linearGrid)
    {
        _linearGrid = linearGrid;
    }

    public LogarithmicGridResult Compute(double start, double stop, int num, double baseValue = 10.0)
    {
        if (num < 0)
        {
            throw new DomainValidationException("logspace: N<0, abort.");
        }

        if (baseValue <= 0.0 || baseValue == 1.0)
        {
            throw new DomainValidationException("logspace: base must be > 0 and != 1.");
        }

        // DEF-002 reproduce-faithfully: exact zero → 1e-12 before log.
        var a = start == 0.0 ? 1e-12 : start;
        var b = stop == 0.0 ? 1e-12 : stop;

        // DEF-003 fix-now: negative (non-zero) endpoints are invalid.
        if (a < 0.0 || b < 0.0)
        {
            throw new DomainValidationException("logspace: start and stop must be non-negative.");
        }

        var logBase = Math.Log(baseValue);
        var logA = Math.Log(a) / logBase;
        var logB = Math.Log(b) / logBase;

        // Both endpoints (istart default true, iend=.true. in Fortran).
        var linear = _linearGrid.Compute(logA, logB, num, includeStart: true, includeStop: true);
        var values = new double[linear.Values.Length];
        for (var i = 0; i < values.Length; i++)
        {
            values[i] = Math.Pow(baseValue, linear.Values[i]);
        }

        return new LogarithmicGridResult { Values = values };
    }
}
