using ErrorOr;

namespace TrainRecord.Common.Errors;

public static class RequestError
{
    public static Error UserIdNotFound =>
        Error.Conflict("Request.UserIdNotFound", "Rota de requisição não possui userId");
}
