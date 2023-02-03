using FluentValidation;

namespace TrainRecord.Application.AuthCommand;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    private readonly string _passwordPattern =
        @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,100})";

    public UpdatePasswordCommandValidator()
    {
        RuleFor(u => u.NewPassword).Matches(_passwordPattern).NotEmpty();
    }
}
