using SciFor.Domain.Ports;

namespace SciFor.Domain.Services;

/// <summary>
/// Fermi–Dirac occupation (XP-108 / FUNCTIONS::fermi). Overflow cutoff: x*beta &gt; 100 → 0.
/// </summary>
public sealed class FermiDiracService : IFermiDirac
{
    public double Evaluate(double x, double beta)
    {
        if (x * beta > 100.0)
        {
            return 0.0;
        }

        return 1.0 / (1.0 + Math.Exp(beta * x));
    }
}
