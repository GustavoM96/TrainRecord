using ErrorOr;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Exceptions
{
    public class RequestException : HandlerException
    {
        public RequestException(Error error) : base(error) { }
    }
}
