using FluentValidation;

namespace TrainRecord.Application.Tests.Common;

public static class AssertExtensions
{
    public static void EqualItems<T>(IEnumerable<T> listOne, IEnumerable<T> listTwo)
    {
        var listOneOrderBy = listOne.OrderBy(item => item);
        var listTwoOrderBy = listTwo.OrderBy(item => item);
        Assert.True(listOneOrderBy.SequenceEqual(listTwoOrderBy));
    }

    public static void AreValidProperties<TValidate>(
        AbstractValidator<TValidate> validator,
        TValidate validateItem
    )
    {
        var validationResults = validator.Validate(validateItem);
        Assert.True(validationResults.IsValid);
    }

    public static void AreInvalidProperties<TValidate>(
        AbstractValidator<TValidate> validator,
        TValidate validateItem,
        params string[] propertyNames
    )
    {
        var validationResults = validator.Validate(validateItem);
        var errorsDistinct = validationResults.Errors.Select(e => e.PropertyName).Distinct();

        EqualItems(propertyNames, errorsDistinct);
    }

    public static void SingleAndEqualItem<T>(IEnumerable<T> listOne, IEnumerable<T> listTwo)
    {
        Assert.Equal(listOne.Single(), listTwo.Single());
    }
}
