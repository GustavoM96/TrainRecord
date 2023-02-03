using ErrorOr;

namespace TrainRecord.Core.Commum
{
    public abstract class HandlerException : Exception
    {
        public List<Error> Errors { get; protected set; }
    }
}
