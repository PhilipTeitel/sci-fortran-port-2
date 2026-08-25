using SciFor.Domain.Models;

namespace SciFor.Domain.Ports;

public interface IFiniteDifference
{
    FiniteDifferenceResult Compute(IReadOnlyList<double> samples, double abscissaSpacing);
}
