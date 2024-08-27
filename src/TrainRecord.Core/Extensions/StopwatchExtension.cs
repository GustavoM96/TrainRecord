using System.Diagnostics;

namespace TrainRecord.Core.Extensions;

public static class StopwatchExtensions
{
    public static TimeSpan GetTime(this Stopwatch stopwatch, Action action)
    {
        stopwatch.Restart();
        action();

        var elapsed = stopwatch.Elapsed;
        stopwatch.Reset();

        return elapsed;
    }

    public static async Task<TimeResult<T>> GetTimeAsync<T>(
        this Stopwatch stopwatch,
        Func<Task<T>> func
    )
    {
        stopwatch.Restart();
        var value = await func();

        var elapsed = stopwatch.Elapsed;
        stopwatch.Reset();

        return new TimeResult<T>(elapsed, value);
    }

    public static async Task<TimeSpan> GetTimeAsync(this Stopwatch stopwatch, Func<Task> func)
    {
        stopwatch.Restart();
        await func();

        var elapsed = stopwatch.Elapsed;
        stopwatch.Reset();

        return elapsed;
    }

    public class TimeResult<T>
    {
        public TimeResult(TimeSpan elapsed, T value)
        {
            Elapsed = elapsed;
            Value = value;
        }

        public TimeSpan Elapsed { get; init; }
        public T Value { get; init; }
    }
}
