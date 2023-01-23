using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using FluentValidation.Results;
using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Exceptions
{
    public class AuthorizationException : HandlerException
    {
        public AuthorizationException(Error error)
        {
            Errors = new List<Error>() { error };
        }
    }
}
