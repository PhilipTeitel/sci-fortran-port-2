using SciFor.Domain.Models;

namespace SciFor.Domain.Ports;

public interface ILogarithmicGrid
{
    LogarithmicGridResult Compute(double start, double stop, int num, double baseValue = 10.0);
}
