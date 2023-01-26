using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
