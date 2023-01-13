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
        public IEnumerable<T> Itens { get; private set; }

        public void AddItens(IEnumerable<T> itens)
        {
            if (Itens is not null)
            {
                throw new PageException(
                    "Não é possível adicionar itens a essa página, pois essa já possui"
                );
            }
            Itens = itens;
        }
    }

    public class Pagination
    {
        public int PageNumber { get; init; } = 1;
        public int PerPage { get; init; } = 1;
    }
}
