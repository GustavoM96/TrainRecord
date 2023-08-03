using FluentValidation;

namespace TrainRecord.Application.ActivityCommand;

public class CreateUserActivityCommandValidator : AbstractValidator<CreateUserActivityCommand>
{
    public CreateUserActivityCommandValidator()
    {
        RuleFor(ua => ua.Repetition).GreaterThan(0);
        RuleFor(ua => ua.Weight).GreaterThanOrEqualTo(0);
        RuleFor(ua => ua.Serie).GreaterThan(0);
    }
}
