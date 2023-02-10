using ErrorOr;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Exceptions
{
    public class PageException : HandlerException
    {
        public PageException(Error error) : base(error) { }
    }
}
