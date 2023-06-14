using System;

namespace startup_shutdown_tracker.Domain;

public record TrackerEntry()
{
	public DateOnly Date { get; init; }
	public DateTimeOffset? StartedAt { get; init; }
	public DateTimeOffset? EndedAt { get; set; }
}