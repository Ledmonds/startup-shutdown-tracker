using System;

namespace startup_shutdown_tracker.Application.Calculators.Hour;

public sealed record HoursEntry(TimeOnly? StartedAt, TimeOnly? EndedAt);