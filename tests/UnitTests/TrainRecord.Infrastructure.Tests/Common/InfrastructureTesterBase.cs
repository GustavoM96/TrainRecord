using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Persistence;
using TrainRecord.Infrastructure.Persistence.Interceptions;

namespace TrainRecord.Infrastructure.Tests.Common;

public abstract class InfrastructureTesterBase
{
    protected static async Task<bool> IsInvalidPropertiesAsync<TValidate>(
        AbstractValidator<TValidate> validator,
        TValidate validateItem,
        params string[] propertyNames
    )
    {
        var validationResults = await validator.ValidateAsync(validateItem);
        var errorsDistinct = validationResults.Errors.Select(e => e.PropertyName).Distinct();

        return EqualItems(propertyNames, errorsDistinct);
    }

    protected static bool EqualItems<T>(IEnumerable<T> listOne, IEnumerable<T> listTwo)
    {
        var listOneOrderBy = listOne.OrderBy(item => item);
        var listTwoOrderBy = listTwo.OrderBy(item => item);

        return listOneOrderBy.SequenceEqual(listTwoOrderBy);
    }

    protected static AppDbContext CreateAppDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDB")
            .Options;

        AuditableEntitySaveChangesInterceptor? interceptor = null!;
        var appDbContext = new AppDbContext(options, interceptor);

        return appDbContext;
    }

    protected static Guid GuidUnique => Guid.NewGuid();

    protected static EntityId<TEntity> EntityIdUnique<TEntity>()
        where TEntity : class, IAuditableEntityBase
    {
        return new EntityId<TEntity>(GuidUnique);
    }

    protected static Pagination PaginationOne => new() { PageNumber = 1, PerPage = 1 };
}
