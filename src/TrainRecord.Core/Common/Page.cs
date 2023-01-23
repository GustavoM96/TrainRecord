using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainRecord.Core.Exceptions;

namespace TrainRecord.Core.Common
{
    public class Page<T> : Pagination
    {
        public IEnumerable<T> Items { get; private set; }

        public void AddItems(IEnumerable<T> itens)
        {
            if (Items is not null)
            {
                throw new PageException(
                    "Não é possível adicionar itens a essa página, pois essa já possui"
                );
            }
            Items = itens;
        }
    }

    public class Pagination
    {
        public int? PageNumber { get; init; }
        public int? PerPage { get; init; }
        public bool NotRequestedPage => PageNumber is null || PerPage is null;
    }
}
