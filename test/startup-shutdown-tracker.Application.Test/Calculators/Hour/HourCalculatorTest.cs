using FluentAssertions;
using startup_shutdown_tracker.Application.Calculators.Hour;
using System;
using System.Collections.Generic;
using Xunit;

namespace startup_shutdown_tracker.Application.Test.Calculators.Hour;

public class HourCalculatorTest
{
	public static IEnumerable<object[]> HoursTestData()
	{
		// Calls with no entries returns 'empty' day calculation
		yield return new object[] { Array.Empty<HoursEntry>(), new HourCalculation(0, 0), };

		// 8:30 - 17:00 (a normal day) causes no overtime.
		yield return new object[]
		{
			new HoursEntry[] { new(new(8, 30, 0), new(17, 0, 0)), },
			new HourCalculation(8, 0),
		};

		// 7:00 - 18:00 causes 2.5 hours of overtime.
		yield return new object[]
		{
			new HoursEntry[] { new(new(7, 0, 0), new(18, 0, 0)), },
			new HourCalculation(10.5, 2.5),
		};

		// 7:00 - 18:00, then 20:00 - 21:00 causes 3.5 hours of overtime.
		yield return new object[]
		{
			new HoursEntry[]
			{
				new(new(7, 0, 0), new(18, 0, 0)),
				new(new(20, 0, 0), new(21, 0, 0)),
			},
			new HourCalculation(11.5, 3.5),
		};

		// 8:00 - 12:00 causes undertime.
		yield return new object[]
		{
			new HoursEntry[] { new(new(8, 0, 0), new(12, 0, 0)), },
			new HourCalculation(3.5, -4.5),
		};
	}

	[Theory]
	[MemberData(nameof(HoursTestData))]
	public void Calculates_hours_for_a_day(
		IEnumerable<HoursEntry> entries,
		HourCalculation expected
	)
	{
		var calculator = new HourCalculator();
		var result = calculator.Calculate(entries);
		result.Should().Be(expected);
	}
}