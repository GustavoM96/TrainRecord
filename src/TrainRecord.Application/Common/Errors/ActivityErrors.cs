﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;

namespace TrainRecord.Application.Errors
{
    public static class ActivityErrors
    {
        public static Error NameExists =>
            Error.Conflict("Activity.NameExists", "nome de atividade já cadastrado");
        public static Error NotFound =>
            Error.NotFound("Activity.NameExists", "atividade não encontrada");
    }
}
