using Throw;
using TrainRecord.Common.Errors;
using TrainRecord.Core.Exceptions;

namespace TrainRecord.Core.Common
{
    public class Page<T> : Pagination
    {
        public IEnumerable<T> Items { get; private set; } = new List<T>();

        public void AddItems(IEnumerable<T> itens)
        {
            Items.Throw(() => new PageException(PageError.AlreadyHasItems)).IfNotEmpty();
            Items = itens;
        }
    }

    public class Pagination
    {
        public int PageNumber { get; init; }
        public int PerPage { get; init; }
    }
}
