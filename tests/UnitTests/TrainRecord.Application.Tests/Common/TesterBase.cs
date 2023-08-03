using FluentValidation;
using TrainRecord.Core.Common;

namespace TrainRecord.Application.Tests.Common;

public abstract class TesterBase
{
    protected static async Task<bool> IsInvalidPropertiesAsync<TValidate>(
        AbstractValidator<TValidate> validator,
        TValidate validateItem,
        params string[] propertyNames
    )
    {
        var validationResults = await validator.ValidateAsync(validateItem);
        var errorsDistinct = validationResults.Errors.Select(e => e.PropertyName).Distinct();

        return SequenceEqual(propertyNames, errorsDistinct);
    }

    protected static bool SequenceEqual<T>(IEnumerable<T> listOne, IEnumerable<T> listTwo)
    {
        var listOneOrderBy = listOne.OrderBy(item => item);
        var listTwoOrderBy = listTwo.OrderBy(item => item);

        return listOneOrderBy.SequenceEqual(listTwoOrderBy);
    }

    protected static Guid GuidUnique => Guid.NewGuid();
    protected static Pagination PaginationOne => new() { PageNumber = 1, PerPage = 1 };
}
