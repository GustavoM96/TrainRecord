using ErrorOr;

namespace TrainRecord.Core.Commum.Bases
{
    public abstract class HandlerException : Exception
    {
        public List<Error> Errors { get; protected set; }
    }
}
