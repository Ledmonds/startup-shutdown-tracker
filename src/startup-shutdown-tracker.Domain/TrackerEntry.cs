using System;

namespace startup_shutdown_tracker.Domain;

public record TrackerEntry()
{
	public DateOnly Date { get; init; }
	public TimeOnly? StartedAt { get; init; }
	public TimeOnly? EndedAt { get; set; }
}