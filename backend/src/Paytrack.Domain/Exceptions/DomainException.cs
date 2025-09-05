﻿namespace Paytrack.Domain.Exceptions;

public abstract class DomainException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}

