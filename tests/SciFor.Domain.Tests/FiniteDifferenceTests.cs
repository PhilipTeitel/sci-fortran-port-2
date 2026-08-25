using SciFor.Domain.Services;

namespace SciFor.Domain.Tests;

public sealed class FiniteDifferenceTests
{
    [Fact]
    public void S20_stencil()
    {
        var sut = new FiniteDifferenceService();

        // Length-2 edge: both ends equal one-sided slope
        var length2 = sut.Compute(new[] { 1.0, 3.0 }, 2.0);
        Assert.Equal(2, length2.Values.Length);
        Assert.Equal(1.0, length2.Values[0]);
        Assert.Equal(1.0, length2.Values[1]);

        // Interior centered difference on f = x^2 at x=0,1,2 with dh=1 → df ≈ 0,2,4? 
        // f=[0,1,4], dh=1: df0=(1-0)/1=1; df1=(4-0)/2=2; df2=(4-1)/1=3
        var quadratic = sut.Compute(new[] { 0.0, 1.0, 4.0 }, 1.0);
        Assert.Equal(1.0, quadratic.Values[0]);
        Assert.Equal(2.0, quadratic.Values[1]);
        Assert.Equal(3.0, quadratic.Values[2]);
    }
}
