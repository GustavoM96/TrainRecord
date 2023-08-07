using ErrorOr;

namespace TrainRecord.Common.Errors;

public static class PageError
{
    public static Error AlreadyHasItems =>
        Error.Conflict(
            "Page.AlreadyHasItems",
            "Não é possível adicionar itens a essa página, pois esta já possui"
        );
}
