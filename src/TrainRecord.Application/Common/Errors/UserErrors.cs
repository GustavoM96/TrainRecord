using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;

namespace TrainRecord.Application.Errors
{
    public static class UserError
    {
        public static Error EmailExists =>
            Error.Conflict("User.EmailExists", "email de usuário já cadastrado");

        public static Error NotFound =>
            Error.NotFound("User.EmailExists", "usuário não encontrado");

        public static Error LoginInvalid =>
            Error.NotFound("User.NotFound", "conta ou senha inválida");
    }
}
