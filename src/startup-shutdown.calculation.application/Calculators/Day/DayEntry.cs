using startup_shutdown.calculation.application.Calculators.Hour;
using System;
using System.Collections.Generic;

namespace startup_shutdown.calculation.application.Calculators.Day;

public sealed record DayEntry(DateOnly Date, IList<HoursEntry> Hours);
