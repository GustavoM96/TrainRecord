using ErrorOr;
using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Exceptions
{
    public class AuthorizationException : HandlerException
    {
        public AuthorizationException(Error error)
        {
            Errors = new List<Error>() { error };
        }
    }
}
