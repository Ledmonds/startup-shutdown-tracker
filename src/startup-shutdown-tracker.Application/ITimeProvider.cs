using System;

namespace startup_shutdown_tracker.Application;

public interface ITimeProvider
{
    DateOnly Date { get; }
    TimeOnly Time { get; }
    DateOnly StartingDateOfTheWeek { get; }
    DateOnly EndingDateOfTheWeek { get; }
}
