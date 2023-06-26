using startup_shutdown.calculation.application.Calculators.Day;
using startup_shutdown.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace startup_shutdown.calculation.application.Calculators.Presentation;

public class Calculator
{
	private readonly IDayEntryRepository _repository;
	private readonly ITimeProvider _timeProvider;
	private readonly DayCalculator _dayCalculator;

	public Calculator(
		IDayEntryRepository repository,
		ITimeProvider timeProvider,
		DayCalculator dayCalculator
	)
	{
		_repository = repository;
		_timeProvider = timeProvider;
		_dayCalculator = dayCalculator;
	}

	public async Task<Calculation> Calculate()
	{
		var dayEntries = await _repository.GetDayEntries();

		var todaysDate = _timeProvider.Date;
		var startingDateOfTheWeek = _timeProvider.StartingDateOfTheWeek;
		var endingDateOfTheWeek = _timeProvider.EndingDateOfTheWeek;

		var calculations = _dayCalculator.Calculate(dayEntries);

		var monthlyCalculations = calculations.Where(
			grouping => grouping.Date.Month == todaysDate.Month
		);

		var weeklyCalculations = calculations.Where(
			grouping =>
				grouping.Date >= startingDateOfTheWeek && grouping.Date <= endingDateOfTheWeek
		);

		var total = ToMetericCalculation(calculations);
		var monthly = ToMetericCalculation(monthlyCalculations);
		var weekly = ToMetericCalculation(weeklyCalculations);

		return new Calculation(total, monthly, weekly);
	}

	private MetricCalculation ToMetericCalculation(IEnumerable<DayCalculation> calculations)
	{
		var hours = calculations.Sum(calculation => calculation.HourCalculation.Worked);
		var overtime = calculations.Sum(calculation => calculation.HourCalculation.Overtime);

		return new MetricCalculation(Math.Round(hours, 2), Math.Round(overtime, 2));
	}
}