using SciFor.Domain.Models;
using SciFor.Domain.Ports;
using SciFor.Domain.Validation;

namespace SciFor.Domain.Services;

/// <summary>
/// Finite-difference derivative (XP-072 / TOOLS::deriv). DEF-005/006 → validation.
/// </summary>
public sealed class FiniteDifferenceService : IFiniteDifference
{
    public FiniteDifferenceResult Compute(IReadOnlyList<double> samples, double abscissaSpacing)
    {
        ArgumentNullException.ThrowIfNull(samples);

        if (samples.Count < 2)
        {
            throw new DomainValidationException("deriv: sampled series length must be >= 2.");
        }

        if (abscissaSpacing == 0.0)
        {
            throw new DomainValidationException("deriv: abscissa spacing dh must be non-zero.");
        }

        var length = samples.Count;
        var df = new double[length];
        var dh = abscissaSpacing;

        df[0] = (samples[1] - samples[0]) / dh;
        for (var i = 1; i < length - 1; i++)
        {
            df[i] = (samples[i + 1] - samples[i - 1]) / (2.0 * dh);
        }

        df[length - 1] = (samples[length - 1] - samples[length - 2]) / dh;

        return new FiniteDifferenceResult { Values = df };
    }
}
