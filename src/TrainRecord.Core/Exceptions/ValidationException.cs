using ErrorOr;
using FluentValidation.Results;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Exceptions
{
    public class ValidationException : HandlerException
    {
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base(failures.Select(f => Error.Validation(f.PropertyName, f.ErrorMessage)).ToList())
        { }
    }
}
