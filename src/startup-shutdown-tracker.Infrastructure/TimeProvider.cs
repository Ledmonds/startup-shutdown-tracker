using startup_shutdown_tracker.Application;
using System;

namespace startup_shutdown_tracker.Infrastructure;

public class TimeProvider : ITimeProvider
{
	public DateOnly Date => DateOnly.FromDateTime(DateTime.UtcNow);
	public TimeOnly Time => TimeOnly.FromDateTime(DateTime.UtcNow.ToLocalTime());
}