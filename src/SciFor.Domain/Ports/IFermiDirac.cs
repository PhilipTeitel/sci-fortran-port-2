namespace SciFor.Domain.Ports;

public interface IFermiDirac
{
    double Evaluate(double x, double beta);
}
