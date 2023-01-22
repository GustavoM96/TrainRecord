using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using FluentValidation.Results;

namespace TrainRecord.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public List<Error> Errors { get; init; }

        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures
                .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
                .ToList();
        }
    }
}
