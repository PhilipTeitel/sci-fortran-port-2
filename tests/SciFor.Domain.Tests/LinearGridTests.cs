using SciFor.Domain.Services;

namespace SciFor.Domain.Tests;

public sealed class LinearGridTests
{
    private readonly LinearGridService _sut = new();

    [Fact]
    public void endpoint_and_mesh_edges()
    {
        // S3 mesh:true returns spacing
        var withMesh = _sut.Compute(0, 1, 5, returnSpacing: true);
        Assert.NotNull(withMesh.Spacing);
        Assert.Equal(0.25, withMesh.Spacing.Value);

        // S4 start-only
        var startOnly = _sut.Compute(0, 1, 4, includeStart: true, includeStop: false);
        Assert.Equal(4, startOnly.Values.Length);
        Assert.Equal(0.0, startOnly.Values[0]);
        Assert.Equal(0.25, startOnly.Values[1]);
        Assert.Equal(0.5, startOnly.Values[2]);
        Assert.Equal(0.75, startOnly.Values[3]);

        // S5 end-only
        var endOnly = _sut.Compute(0, 1, 4, includeStart: false, includeStop: true);
        Assert.Equal(0.25, endOnly.Values[0]);
        Assert.Equal(1.0, endOnly.Values[3]);

        // S6 neither
        var neither = _sut.Compute(0, 1, 3, includeStart: false, includeStop: false);
        Assert.Equal(0.25, neither.Values[0]);
        Assert.Equal(0.5, neither.Values[1]);
        Assert.Equal(0.75, neither.Values[2]);

        // S7 decreasing
        var decreasing = _sut.Compute(1, 0, 5);
        Assert.Equal(1.0, decreasing.Values[0]);
        Assert.Equal(0.0, decreasing.Values[4]);

        // S8 constant
        var constant = _sut.Compute(2, 2, 3);
        Assert.All(constant.Values, v => Assert.Equal(2.0, v));
    }
}
