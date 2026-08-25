namespace SciFor.Parity.Tests;

/// <summary>
/// Per-path comparison rule from XP-060/061/072/108 test plans / DEC-005:
/// |a-b| &lt;= max(1e-12, 1e-12 * max(|a|, |b|)).
/// </summary>
public static class NumericTolerance
{
    public static bool NearlyEqual(double actual, double expected)
    {
        var absDiff = Math.Abs(actual - expected);
        var bound = Math.Max(1e-12, 1e-12 * Math.Max(Math.Abs(actual), Math.Abs(expected)));
        return absDiff <= bound;
    }

    public static void AssertSequenceEqual(IReadOnlyList<double> actual, IReadOnlyList<double> expected)
    {
        Assert.Equal(expected.Count, actual.Count);
        for (var i = 0; i < expected.Count; i++)
        {
            Assert.True(
                NearlyEqual(actual[i], expected[i]),
                $"Index {i}: actual={actual[i]:R} expected={expected[i]:R} |diff|={Math.Abs(actual[i] - expected[i]):R}");
        }
    }
}
