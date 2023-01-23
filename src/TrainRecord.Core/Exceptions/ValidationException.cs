using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using FluentValidation.Results;
using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Exceptions
{
    public class ValidationException : HandlerException
    {
        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures
                .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
                .ToList();
        }
    }
}