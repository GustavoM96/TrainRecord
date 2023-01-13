using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using TrainRecord.Core.Common;

namespace TrainRecord.Application.Common.Extentions
{
    public static class QuerableExtentions
    {
        public static Page<T> Pagination<T>(this IQueryable<T> queryable, Pagination pagination)
        {
            var pageToSkip = (pagination.PageNumber - 1) * pagination.PerPage;
            var itens = queryable.Skip(pageToSkip).Take(pagination.PerPage);

            var page = pagination.Adapt<Page<T>>();
            page.AddItens(itens);

            return page;
        }
    }
}
