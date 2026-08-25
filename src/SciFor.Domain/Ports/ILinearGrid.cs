using SciFor.Domain.Models;

namespace SciFor.Domain.Ports;

public interface ILinearGrid
{
    LinearGridResult Compute(
        double start,
        double stop,
        int num,
        bool includeStart = true,
        bool includeStop = true,
        bool returnSpacing = false);
}
