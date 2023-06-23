using System.Collections.Generic;
using System.Linq;

namespace startup_shutdown.calculation.application.Calculators.Hour;

public class HourCalculator
{
    private const double _hoursInDay = 8;
    private const double _lunchBreak = 0.5;

    public virtual HourCalculation Calculate(IEnumerable<HoursEntry> hourEntries)
    {
        if (!hourEntries.Any())
        {
            return new(0, 0);
        }

        var totalTrackedHours = hourEntries.Sum(
            entry => ((entry.EndedAt - entry.StartedAt)?.TotalMinutes ?? 0) / 60
        );

        var totalHoursWorked = totalTrackedHours - _lunchBreak;
        var overtime = totalHoursWorked - _hoursInDay;

        return new(totalHoursWorked, overtime);
    }
}
