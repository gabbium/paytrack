using Paytrack.Domain.Resources;

namespace Paytrack.Application.UseCases.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage(Resource.User_Email_NotEmpty)
            .EmailAddress()
            .WithMessage(Resource.User_Email_EmailAddress)
            .MaximumLength(256)
            .WithMessage(string.Format(Resource.User_Email_MaxLength, 256));

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage(Resource.User_Password_NotEmpty)
            .MinimumLength(6)
            .WithMessage(string.Format(Resource.User_Password_MinLength, 6))
            .MaximumLength(128)
            .WithMessage(string.Format(Resource.User_Password_MaxLength, 128));
    }
}
