using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TrainRecord.Core.Extentions
{
    public static class StopwatchExtention
    {
        public static TimeSpan GetTime(this Stopwatch stopwatch, Action action)
        {
            stopwatch.Start();
            action();

            var elapsed = stopwatch.Elapsed;
            stopwatch.Reset();

            return elapsed;
        }

        public static async Task<TimeSpan> GetTimeAsync(this Stopwatch stopwatch, Func<Task> func)
        {
            stopwatch.Start();
            await func();

            var elapsed = stopwatch.Elapsed;
            stopwatch.Reset();

            return elapsed;
        }
    }
}
