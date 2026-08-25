using SciFor.Domain.Ports;
using SciFor.Domain.Services;
using SciFor.Domain.Validation;

namespace SciFor.Domain.Tests;

/// <summary>
/// Port contract for <see cref="ILinearGrid"/> — any adapter of this port must satisfy these cases.
/// </summary>
public sealed class LinearGridPortContractTests
{
    public static IEnumerable<object[]> Implementations()
    {
        yield return [new LinearGridService()];
    }

    [Theory]
    [MemberData(nameof(Implementations))]
    public void happy_path_both_endpoints(ILinearGrid port)
    {
        var result = port.Compute(0, 1, 5);
        Assert.Equal(5, result.Values.Length);
        Assert.Equal(0.0, result.Values[0]);
        Assert.Equal(1.0, result.Values[4]);
    }

    [Theory]
    [MemberData(nameof(Implementations))]
    public void rejects_invalid_num(ILinearGrid port)
    {
        Assert.Throws<DomainValidationException>(() => port.Compute(0, 1, -1));
        Assert.Throws<DomainValidationException>(() => port.Compute(0, 1, 1));
        Assert.Throws<DomainValidationException>(() => port.Compute(0, 1, 0, includeStart: true, includeStop: false));
    }
}
