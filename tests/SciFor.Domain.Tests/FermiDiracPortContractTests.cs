using SciFor.Domain.Ports;
using SciFor.Domain.Services;

namespace SciFor.Domain.Tests;

public sealed class FermiDiracPortContractTests
{
    public static IEnumerable<object[]> Implementations()
    {
        yield return [new FermiDiracService()];
    }

    [Theory]
    [MemberData(nameof(Implementations))]
    public void formula_and_cutoff(IFermiDirac port)
    {
        Assert.Equal(0.5, port.Evaluate(0, 100));
        Assert.Equal(0.0, port.Evaluate(2, 100));
        Assert.Equal(0.5, port.Evaluate(5, 0));
        Assert.True(port.Evaluate(1, 100) > 0.0);
    }
}
