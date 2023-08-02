using FluentValidation;

namespace TrainRecord.Application.ActivityCommand;

public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
{
    public CreateActivityCommandValidator()
    {
        RuleFor(a => a.Name).NotEmpty();
    }
}
