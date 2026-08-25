using SciFor.Domain.Ports;
using SciFor.Domain.Services;

namespace SciFor.Parity.Tests;

public sealed class Xp060ParityTests
{
    // Expected values from oracle.md v1 / XP-388 pin (E1), also present in
    // .oracle-sandbox/probe-out/run1.out section === linspace-5 ===.
    // Source grade: E1 verified (oracle.md §1; XP-060 path detail §1 step 10).
    private static readonly double[] ExpectedLinspace01Num5 =
    [
        0.0,
        0.25,
        0.5,
        0.75,
        1.0,
    ];

    [Fact]
    public void parity_XP060_P1()
    {
        ILinearGrid grid = new LinearGridService();
        var result = grid.Compute(start: 0.0, stop: 1.0, num: 5);
        NumericTolerance.AssertSequenceEqual(result.Values, ExpectedLinspace01Num5);
    }
}
