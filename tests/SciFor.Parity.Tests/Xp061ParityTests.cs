using SciFor.Domain.Ports;
using SciFor.Domain.Services;

namespace SciFor.Parity.Tests;

public sealed class Xp061ParityTests
{
    // Expected values from live T1 probe stdout (.oracle-sandbox/probe-out/run1.out
    // === logspace-5 ===), oracle.md v1 / XP-388 pin. Source grade: E1 verified.
    // Note: these are the printed es24.17 doubles from gfortran, not a recomputed
    // ideal; path-detail prose rounded slightly differently.
    private static readonly double[] ExpectedLogspace1To1000Num5 =
    [
        1.00000000000000000e+00,
        5.62341325190348940e+00,
        3.16227766016837784e+01,
        1.77827941003892107e+02,
        9.99999999999998977e+02,
    ];

    [Fact]
    public void parity_XP061_P2()
    {
        ILogarithmicGrid grid = new LogarithmicGridService();
        var result = grid.Compute(start: 1.0, stop: 1000.0, num: 5);
        NumericTolerance.AssertSequenceEqual(result.Values, ExpectedLogspace1To1000Num5);
    }
}
