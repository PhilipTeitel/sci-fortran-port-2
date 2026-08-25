using SciFor.Domain.Ports;
using SciFor.Domain.Services;
using SciFor.Domain.Validation;

namespace SciFor.Domain.Tests;

public sealed class LogarithmicGridPortContractTests
{
    public static IEnumerable<object[]> Implementations()
    {
        yield return [new LogarithmicGridService()];
    }

    [Theory]
    [MemberData(nameof(Implementations))]
    public void happy_path_default_base(ILogarithmicGrid port)
    {
        var result = port.Compute(1, 1000, 5);
        Assert.Equal(5, result.Values.Length);
        Assert.Equal(1.0, result.Values[0], precision: 12);
    }

    [Theory]
    [MemberData(nameof(Implementations))]
    public void rejects_invalid_inputs(ILogarithmicGrid port)
    {
        Assert.Throws<DomainValidationException>(() => port.Compute(-1, 10, 5));
        Assert.Throws<DomainValidationException>(() => port.Compute(1, 10, 5, baseValue: 1.0));
        Assert.Throws<DomainValidationException>(() => port.Compute(1, 10, -1));
        Assert.Throws<DomainValidationException>(() => port.Compute(1, 10, 1));
    }
}
