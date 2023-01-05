using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;

namespace TrainRecord.Application.Errors
{
    public static class ActivityErrors
    {
        public static Error NameExists =>
            Error.Conflict("Activity.NameExists", "já existe um nome cadastrado");
    }
}
