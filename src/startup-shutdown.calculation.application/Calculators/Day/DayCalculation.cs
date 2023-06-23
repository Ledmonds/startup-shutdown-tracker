using System;

namespace startup_shutdown.calculation.application.Calculators.Day;

public record DayCalculation(DateOnly Date, double Worked, double OverTime);
