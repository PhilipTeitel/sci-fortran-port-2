using SciFor.Domain.Ports;
using SciFor.Domain.Services;

namespace SciFor.Parity.Tests;

public sealed class Xp108ParityTests
{
    // Five-point beta=100 sample from oracle.md v1 / XP-388 pin (E1),
    // also in .oracle-sandbox/probe-out/run1.out === fermi-beta100 ===.
    private static readonly (double X, double Value)[] ExpectedFermiBeta100 =
    [
        (-2.0, 1.0),
        (-1.0, 1.0),
        (0.0, 0.5),
        (1.0, 3.72007597602083562e-44),
        (2.0, 0.0),
    ];

    [Fact]
    public void parity_XP108_P4()
    {
        IFermiDirac fermi = new FermiDiracService();
        const double beta = 100.0;
        foreach (var (x, expected) in ExpectedFermiBeta100)
        {
            var actual = fermi.Evaluate(x, beta);
            Assert.True(
                NumericTolerance.NearlyEqual(actual, expected),
                $"x={x}: actual={actual:R} expected={expected:R}");
        }
    }
}
