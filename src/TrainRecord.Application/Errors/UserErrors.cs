﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;

namespace TrainRecord.Application.Errors
{
    public static class UserError
    {
        public static Error EmailExists =>
            Error.Conflict("User.EmailExists", "já esxiste um email cadastrado");
    }
}
