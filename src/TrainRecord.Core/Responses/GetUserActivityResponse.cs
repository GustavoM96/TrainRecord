using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainRecord.Core.Responses
{
    public class GetUserActivityResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }
}
