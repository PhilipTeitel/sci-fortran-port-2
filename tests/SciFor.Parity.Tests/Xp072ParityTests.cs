using SciFor.Domain.Ports;
using SciFor.Domain.Services;

namespace SciFor.Parity.Tests;

public sealed class Xp072ParityTests
{
    [Fact]
    public void parity_XP072_P3()
    {
        // Input y and expected dy from XP-388 / oracle.md v1 pin:
        // y copied from numutils/test/xy2.data; dy from probe-out/run1.out === deriv-xy2 ===.
        // Source grade: E1 verified. dh = x(2)-x(1) from the same file.
        var fixtureDir = Path.Combine(AppContext.BaseDirectory, "Fixtures");
        var y = File.ReadAllLines(Path.Combine(fixtureDir, "xp072-y.txt"))
            .Where(static l => !string.IsNullOrWhiteSpace(l))
            .Select(double.Parse)
            .ToArray();
        var x = File.ReadAllLines(Path.Combine(fixtureDir, "xp072-x.txt"))
            .Where(static l => !string.IsNullOrWhiteSpace(l))
            .Select(double.Parse)
            .ToArray();
        var expectedDy = File.ReadAllLines(Path.Combine(fixtureDir, "xp072-dy-expected.txt"))
            .Where(static l => !string.IsNullOrWhiteSpace(l))
            .Select(double.Parse)
            .ToArray();

        Assert.True(x.Length >= 2);
        var dh = x[1] - x[0];

        IFiniteDifference deriv = new FiniteDifferenceService();
        var result = deriv.Compute(y, dh);
        NumericTolerance.AssertSequenceEqual(result.Values, expectedDy);
    }
}
