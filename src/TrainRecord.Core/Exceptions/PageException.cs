using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainRecord.Core.Exceptions
{
    public class PageException : Exception
    {
        public PageException(string message) : base(message) { }
    }
}
