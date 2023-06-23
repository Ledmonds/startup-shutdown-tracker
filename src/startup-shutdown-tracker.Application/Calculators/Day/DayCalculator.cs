using startup_shutdown_tracker.Application.Calculators.Hour;
using System.Collections.Generic;
using System.Linq;

namespace startup_shutdown_tracker.Application.Calculators.Day;

public class DayCalculator
{
	private readonly HourCalculator _hourCalculator;

	public DayCalculator(HourCalculator hourCalculator)
	{
		_hourCalculator = hourCalculator;
	}

	public IReadOnlyCollection<DayCalculation> Calculate(IEnumerable<DayEntry> dayEntries)
	{
		var calculations = dayEntries
			.Select(entry =>
			{
				var calculation = _hourCalculator.Calculate(entry.Hours);
				return new DayCalculation(entry.Date, calculation.Worked, calculation.Overtime);
			})
			.ToArray();

		return calculations;
	}
}