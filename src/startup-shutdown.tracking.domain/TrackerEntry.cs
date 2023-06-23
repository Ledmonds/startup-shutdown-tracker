using System;

namespace startup_shutdown_tracker.Domain;

public record TrackerEntry
{
	public required Guid Id { get; init; }
	public required DateOnly Date { get; init; }
	public required TimeOnly? StartedAt { get; set; }
	public required TimeOnly? EndedAt { get; set; }
}