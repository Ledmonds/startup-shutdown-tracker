using startup_shutdown.calculation.application.Calculators.Hour;
using System;

namespace startup_shutdown.calculation.application.Calculators.Day;

public record DayCalculation(DateOnly Date, HourCalculation HourCalculation);