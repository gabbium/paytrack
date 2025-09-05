namespace Paytrack.Domain.Exceptions;

public sealed class DomainRuleViolation(string code, string message) : DomainException(code, message);

