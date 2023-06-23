using startup_shutdown_tracker.Application.Calculators.Hour;
using System;
using System.Collections.Generic;

namespace startup_shutdown_tracker.Application.Calculators.Day;

public sealed record DayEntry(DateOnly Date, IList<HoursEntry> Hours);