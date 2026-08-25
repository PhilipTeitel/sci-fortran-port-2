using SciFor.Domain.Ports;
using SciFor.Domain.Services;
using SciFor.Domain.Validation;

namespace SciFor.Domain.Tests;

public sealed class FiniteDifferencePortContractTests
{
    public static IEnumerable<object[]> Implementations()
    {
        yield return [new FiniteDifferenceService()];
    }

    [Theory]
    [MemberData(nameof(Implementations))]
    public void happy_path_length_match(IFiniteDifference port)
    {
        var result = port.Compute(new[] { 0.0, 1.0, 2.0 }, 1.0);
        Assert.Equal(3, result.Values.Length);
    }

    [Theory]
    [MemberData(nameof(Implementations))]
    public void rejects_short_or_zero_dh(IFiniteDifference port)
    {
        Assert.Throws<DomainValidationException>(() => port.Compute(new[] { 1.0 }, 0.1));
        Assert.Throws<DomainValidationException>(() => port.Compute(new[] { 1.0, 2.0 }, 0.0));
    }
}
