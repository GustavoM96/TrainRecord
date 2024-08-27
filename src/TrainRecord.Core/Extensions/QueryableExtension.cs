using Mapster;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Extensions;

public static class QueryableExtensions
{
    public static Page<T> AsPage<T>(this IQueryable<T> queryable, Pagination pagination)
    {
        var items = queryable.GetItemsByPagination(pagination).ToList();
        var page = pagination.Adapt<Page<T>>();
        page.AddItems(items);

        return page;
    }

    public static Page<TAdapt> AsPageAdapted<T, TAdapt>(
        this IQueryable<T> queryable,
        Pagination pagination
    )
    {
        var items = queryable.GetItemsByPagination(pagination).ProjectToType<TAdapt>().ToList();
        var page = pagination.Adapt<Page<TAdapt>>();
        page.AddItems(items);

        return page;
    }

    public static IQueryable<T> GetItemsByPagination<T>(
        this IQueryable<T> queryable,
        Pagination pagination
    )
    {
        if (pagination.IsNotRequestedPage())
        {
            return queryable;
        }

        var perPage = pagination.PerPage!.Value;
        var pageNumber = pagination.PageNumber!.Value;

        var pageToSkip = (pageNumber - 1) * perPage;
        return queryable.Skip(pageToSkip).Take(perPage);
    }
}
