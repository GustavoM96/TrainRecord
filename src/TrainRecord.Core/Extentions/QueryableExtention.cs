using Mapster;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Extentions
{
    public static class QueryableExtention
    {
        public static Page<T> AsPage<T>(this IQueryable<T> queryable, Pagination pagination)
        {
            var items = queryable.GetItemsByPagination(pagination);
            var page = pagination.Adapt<Page<T>>();
            page.AddItems(items);

            return page;
        }

        public static Page<TAdapt> AsPageAdapted<T, TAdapt>(
            this IQueryable<T> queryable,
            Pagination pagination
        )
        {
            var items = queryable.GetItemsByPagination(pagination).Select(i => i.Adapt<TAdapt>());
            var page = pagination.Adapt<Page<TAdapt>>();
            page.AddItems(items);

            return page;
        }

        public static IQueryable<T> GetItemsByPagination<T>(
            this IQueryable<T> queryable,
            Pagination pagination
        )
        {
            if (pagination is null)
            {
                return queryable;
            }

            var perPage = pagination.PerPage;
            var pageNumber = pagination.PageNumber;

            var pageToSkip = (pageNumber - 1) * perPage;
            return queryable.Skip(pageToSkip).Take(perPage);
        }
    }
}
