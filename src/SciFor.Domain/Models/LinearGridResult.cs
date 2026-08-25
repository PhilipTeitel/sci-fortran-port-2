namespace SciFor.Domain.Models;

public sealed class LinearGridResult
{
    public required double[] Values { get; init; }

    public double? Spacing { get; init; }
}
