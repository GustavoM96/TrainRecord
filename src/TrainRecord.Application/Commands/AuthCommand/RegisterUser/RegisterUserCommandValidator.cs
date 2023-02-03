using FluentValidation;

namespace TrainRecord.Application.AuthCommand;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly string _passwordPattern =
        @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,100})";

    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.Email).EmailAddress().NotEmpty();
        RuleFor(u => u.Password).Matches(_passwordPattern).NotEmpty();
    }
}
