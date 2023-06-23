using System;

namespace startup_shutdown.tracking.domain;

public record TrackerEntry
{
    public required Guid Id { get; init; }
    public required DateOnly Date { get; init; }
    public required TimeOnly? StartedAt { get; set; }
    public required TimeOnly? EndedAt { get; set; }
}
