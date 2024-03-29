using FluentValidation;
using TrainRecord.Core.Common;

namespace TrainRecord.Application.Common.Validations;

public class PageValidatorValidator : AbstractValidator<Pagination>
{
    public PageValidatorValidator()
    {
        RuleFor(a => a.PageNumber).GreaterThan(0);
        RuleFor(a => a.PerPage).GreaterThan(0);
    }
}
