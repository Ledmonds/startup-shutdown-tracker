using System;

namespace startup_shutdown.calculation.application.Calculators.Hour;

public sealed record HoursEntry(TimeOnly? StartedAt, TimeOnly? EndedAt);
