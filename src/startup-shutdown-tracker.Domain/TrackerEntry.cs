using System;

namespace startup_shutdown_tracker.Domain;

public record TrackerEntry(DateOnly Date, TimeOnly? StartedAt, TimeOnly? EndedAt);