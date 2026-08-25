namespace SciFor.Domain.Validation;

/// <summary>
/// Domain validation failure mapped to HTTP 400 Problem Details by the API adapter.
/// </summary>
public sealed class DomainValidationException : Exception
{
    public DomainValidationException(string message)
        : base(message)
    {
    }
}
