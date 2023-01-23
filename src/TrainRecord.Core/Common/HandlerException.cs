using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using FluentValidation.Results;

namespace TrainRecord.Core.Commum
{
    public abstract class HandlerException : Exception
    {
        public List<Error> Errors { get; protected set; }
    }
}
