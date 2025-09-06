namespace Paytrack.Domain.Errors;

public static class UserErrors
{
    public static readonly Error EmptyEmail = Error.Validation("The email cannot be empty or whitespace.");
    public static readonly Error EmptyPasswordHash = Error.Validation("The password hash cannot be empty or whitespace.");
    public static readonly Error EmptyCurrency = Error.Validation("The currency code cannot be empty or whitespace.");
    public static readonly Error EmptyTimeZone = Error.Validation("The time zone cannot be empty or whitespace.");
    public static readonly Error EmailAlreadyInUse = Error.Conflict("The specified email is already in use.");
    public static readonly Error InvalidCredentials = Error.Unauthorized("Invalid email or password.");
}
