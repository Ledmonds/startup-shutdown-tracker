using System;

namespace startup_shutdown_tracker.Application;

public interface ITimeProvider
{
	DateOnly Date { get; }
	DateTimeOffset UtcNow { get; }
}