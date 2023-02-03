using ErrorOr;

namespace TrainRecord.Application.Errors
{
    public static class UserActivityErrors
    {
        public static Error NotFound =>
            Error.NotFound(
                "UserActivityErrors.NameExists",
                "não encontrado record para essa atividade"
            );
    }
}
