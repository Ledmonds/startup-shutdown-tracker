using FluentAssertions;
using Moq;
using startup_shutdown.calculation.application.Calculators.Day;
using startup_shutdown.calculation.application.Calculators.Hour;
using Xunit;

namespace Calculators;

public class DayCalculatorTest
{
	[Fact]
	public void Passes_hours_for_a_given_day_through_to_HoursCalculator()
	{
		var hoursCalculator = new Mock<HourCalculator>();
		var dayCalculator = new DayCalculator(hoursCalculator.Object);

		var firstDaysHours = new[]
		{
			new HoursEntry(new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0)),
			new HoursEntry(new TimeOnly(20, 0, 0), new TimeOnly(22, 0, 0)),
		};

		var secondDaysHours = new[]
		{
			new HoursEntry(new TimeOnly(9, 0, 0), new TimeOnly(17, 0, 0)),
		};

		var days = new DayEntry[]
		{
			new(new DateOnly(2021, 1, 1), firstDaysHours),
			new(new DateOnly(2023, 12, 2), secondDaysHours),
		};

		var firstHoursCalculation = new HourCalculation(10, 20);
		hoursCalculator.Setup(x => x.Calculate(firstDaysHours)).Returns(firstHoursCalculation);

		var secondHoursCalculation = new HourCalculation(5, 10);
		hoursCalculator.Setup(x => x.Calculate(secondDaysHours)).Returns(secondHoursCalculation);

		// Act
		var result = dayCalculator.Calculate(days);

		// Assert
		var expected = new[]
		{
			new DayCalculation(new DateOnly(2021, 1, 1), firstHoursCalculation),
			new DayCalculation(new DateOnly(2023, 12, 2), secondHoursCalculation),
		};

		result.Should().BeEquivalentTo(expected);
	}
}