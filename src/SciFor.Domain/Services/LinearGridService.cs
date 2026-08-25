using SciFor.Domain.Models;
using SciFor.Domain.Ports;
using SciFor.Domain.Validation;

namespace SciFor.Domain.Services;

/// <summary>
/// Linear grid (XP-060 / TOOLS::linspace), formulas from path detail §1.
/// </summary>
public sealed class LinearGridService : ILinearGrid
{
    public LinearGridResult Compute(
        double start,
        double stop,
        int num,
        bool includeStart = true,
        bool includeStop = true,
        bool returnSpacing = false)
    {
        if (num < 0)
        {
            throw new DomainValidationException("linspace: N<0, abort.");
        }

        if (includeStart && includeStop && num < 2)
        {
            throw new DomainValidationException("linspace: N<2 with both start and end points");
        }

        // DEF-001 fix-now: num==0 with an endpoint excluded must not divide by zero.
        if (num == 0 && (!includeStart || !includeStop))
        {
            throw new DomainValidationException("linspace: N=0 with an endpoint excluded is invalid.");
        }

        double step;
        var values = new double[num];

        if (includeStart && includeStop)
        {
            step = (stop - start) / (num - 1);
            for (var i = 0; i < num; i++)
            {
                values[i] = start + i * step;
            }
        }
        else if (includeStart && !includeStop)
        {
            step = (stop - start) / num;
            for (var i = 0; i < num; i++)
            {
                values[i] = start + i * step;
            }
        }
        else if (!includeStart && includeStop)
        {
            step = (stop - start) / num;
            for (var i = 0; i < num; i++)
            {
                values[i] = start + (i + 1) * step;
            }
        }
        else
        {
            step = (stop - start) / (num + 1);
            for (var i = 0; i < num; i++)
            {
                values[i] = start + (i + 1) * step;
            }
        }

        return new LinearGridResult
        {
            Values = values,
            Spacing = returnSpacing ? step : null,
        };
    }
}
