using System;

namespace startup_shutdown_tracker.Application.Calculators.Day;

public record DayCalculation(DateOnly Date, double Worked, double OverTime);