using SciFor.Domain.Services;

namespace SciFor.Domain.Tests;

public sealed class LogarithmicGridTests
{
    [Fact]
    public void S14_zero_rewrite()
    {
        var sut = new LogarithmicGridService();
        var result = sut.Compute(start: 0.0, stop: 1.0, num: 3, baseValue: 10.0);
        Assert.Equal(3, result.Values.Length);
        // DEF-002: start 0 → 1e-12; first value ≈ 1e-12 when both endpoints.
        Assert.True(Math.Abs(result.Values[0] - 1e-12) <= 1e-12 * Math.Max(Math.Abs(result.Values[0]), 1e-12));
        Assert.True(Math.Abs(result.Values[2] - 1.0) <= 1e-12);
    }
}
