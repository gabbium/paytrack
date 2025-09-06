namespace Paytrack.Domain.Errors;

public static class MovementErrors
{
    public static readonly Error EmptyUserId = Error.Validation("The user id cannot be empty or whitespace.");
    public static readonly Error InvalidAmount = Error.Validation("The amount must be greater than zero.");
    public static readonly Error NotFound = Error.NotFound("The specified movement was not found.");
}
