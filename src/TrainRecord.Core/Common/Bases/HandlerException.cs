using ErrorOr;

namespace TrainRecord.Core.Commum.Bases;

public abstract class HandlerException : Exception
{
    public HandlerException(Error error)
        : base(error.Description)
    {
        Errors = new() { error };
    }

    public HandlerException(List<Error> errors)
        : base(string.Join(". ", errors.Select(e => e.Description)))
    {
        Errors = errors;
    }

    public List<Error> Errors { get; private set; }
}
