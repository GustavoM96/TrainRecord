using ErrorOr;

namespace TrainRecord.Core.Commum.Bases
{
    public abstract class HandlerException : Exception
    {
        public HandlerException(Error error) : base(ErrorMessage)
        {
            Errors = new List<Error>() { error };
        }

        public HandlerException(List<Error> errors) : base(ErrorMessage)
        {
            Errors = errors;
        }

        private const string ErrorMessage = "a handle error has occurred.";
        public List<Error> Errors { get; private set; }
    }
}
