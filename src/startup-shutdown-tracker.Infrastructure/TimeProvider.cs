using startup_shutdown_tracker.Application;
using System;

namespace startup_shutdown_tracker.Infrastructure;

public class TimeProvider : ITimeProvider
{
    public DateOnly Date => DateOnly.FromDateTime(DateTime.UtcNow);
    public TimeOnly Time => TimeOnly.FromDateTime(DateTime.UtcNow.ToLocalTime());

    public DateOnly StartingDateOfTheWeek => Date.AddDays(-DayOfTheWeek());

    public DateOnly EndingDateOfTheWeek => StartingDateOfTheWeek.AddDays(6);

    private int DayOfTheWeek()
    {
        var dayOfWeek = Date.DayOfWeek;

        return dayOfWeek switch
        {
            DayOfWeek.Sunday => 6,
            _ => (int)dayOfWeek - 1,
        };
    }
}
