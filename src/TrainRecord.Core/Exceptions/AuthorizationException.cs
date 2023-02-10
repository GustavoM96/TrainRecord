using ErrorOr;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Exceptions
{
    public class AuthorizationException : HandlerException
    {
        public AuthorizationException(Error error) : base(error) { }
    }
}
