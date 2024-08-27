using AutoFixture;
using AutoFixture.AutoMoq;
using FluentValidation;
using TrainRecord.Application.AuthCommand;
using TrainRecord.Core.Common;
using TrainRecord.Core.Enum;

namespace TrainRecord.Application.Tests.Common;

public abstract class ApplicationTesterBase
{
    public ApplicationTesterBase()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization());
        fixture.Customize<Pagination>(c => c.With(c => c.PageNumber, 1).With(c => c.PerPage, 1));
        fixture.Customize<RegisterUserCommand>(c => c.With(c => c.Role, Role.User));

        _fixture = fixture;
    }

    protected static async Task<bool> AreInvalidPropertiesAsync<TValidate>(
        AbstractValidator<TValidate> validator,
        TValidate validateItem,
        params string[] propertyNames
    )
    {
        var validationResults = await validator.ValidateAsync(validateItem);
        var errorsDistinct = validationResults.Errors.Select(e => e.PropertyName).Distinct();

        return EqualItems(propertyNames, errorsDistinct);
    }

    protected static async Task<bool> AreValidPropertiesAsync<TValidate>(
        AbstractValidator<TValidate> validator,
        TValidate validateItem
    )
    {
        var validationResults = await validator.ValidateAsync(validateItem);
        return validationResults.IsValid;
    }

    protected static bool EqualItems<T>(IEnumerable<T> listOne, IEnumerable<T> listTwo)
    {
        var listOneOrderBy = listOne.OrderBy(item => item);
        var listTwoOrderBy = listTwo.OrderBy(item => item);

        return listOneOrderBy.SequenceEqual(listTwoOrderBy);
    }

    protected static Guid GuidUnique => Guid.NewGuid();
    protected static Pagination PaginationOne => new() { PageNumber = 1, PerPage = 1 };

    protected T CreateFixture<T>() => _fixture.Create<T>();

    protected T FreezeFixture<T>() => _fixture.Freeze<T>();

    private readonly Fixture _fixture;
}
