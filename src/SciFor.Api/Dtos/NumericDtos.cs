namespace SciFor.Api.Dtos;

public sealed class LinspaceRequest
{
    public double? Start { get; set; }

    public double? Stop { get; set; }

    public int? Num { get; set; }

    public bool? Istart { get; set; }

    public bool? Iend { get; set; }

    /// <summary>Request flag: when true, response includes numeric spacing as <c>mesh</c>.</summary>
    public bool? Mesh { get; set; }
}

public sealed class LinspaceResponse
{
    public required double[] Values { get; init; }

    public double? Mesh { get; init; }
}

public sealed class LogspaceRequest
{
    public double? Start { get; set; }

    public double? Stop { get; set; }

    public int? Num { get; set; }

    public double? Base { get; set; }
}

public sealed class ValuesResponse
{
    public required double[] Values { get; init; }
}

public sealed class DerivRequest
{
    public double[]? F { get; set; }

    public double? Dh { get; set; }
}

public sealed class DerivResponse
{
    public required double[] Df { get; init; }
}

public sealed class FermiRequest
{
    public double? X { get; set; }

    public double? Beta { get; set; }
}

public sealed class FermiResponse
{
    public required double Value { get; init; }
}
