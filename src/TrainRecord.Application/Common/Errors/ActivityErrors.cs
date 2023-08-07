using ErrorOr;

namespace TrainRecord.Application.Errors;

public static class ActivityErrors
{
    public static Error NameAlreadyExists =>
        Error.Conflict("Activity.NameExists", "nome de atividade já cadastrado");
    public static Error NotFound =>
        Error.NotFound("Activity.NameExists", "atividade não encontrada");
}
