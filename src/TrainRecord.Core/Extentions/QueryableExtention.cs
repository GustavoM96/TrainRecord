using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Extentions
{
    public static class QueryableExtention
    {
        public static Page<T> GetPage<T>(this IQueryable<T> queryable, Pagination pagination)
        {
            var items = queryable.GetItemsByPagination(pagination);
            var page = pagination.Adapt<Page<T>>();
            page.AddItems(items);

            return page;
        }

        public static Page<TAdapt> GetPage<T, TAdapt>(
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
            var pageToSkip = (pagination.PageNumber - 1) * pagination.PerPage;
            return queryable.Skip(pageToSkip).Take(pagination.PerPage);
        }
    }
}
