using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace TrainRecord.Application.Tests.Common;

public abstract class TesterBase
{
    protected async Task<bool> IsInvalidPropertiesAsync<TValidate>(
        AbstractValidator<TValidate> validator,
        TValidate validateItem,
        params string[] propertyNames
    )
    {
        var propertyNamesOrdered = propertyNames.OrderBy(x => x);

        var validationResults = await validator.ValidateAsync(validateItem);
        var errorsOrdered = validationResults.Errors
            .Select(e => e.PropertyName)
            .Distinct()
            .OrderBy(e => e);

        return propertyNamesOrdered.SequenceEqual(errorsOrdered);
    }

    protected static Guid GuidUnique => Guid.NewGuid();
}