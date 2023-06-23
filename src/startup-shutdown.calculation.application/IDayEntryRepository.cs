using startup_shutdown.calculation.application.Calculators.Day;
using startup_shutdown.calculation.application.Calculators.Hour;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace startup_shutdown.calculation.application;

public interface IDayEntryRepository
{
	Task<IReadOnlyCollection<DayEntry>> GetDayEntries();

	Task<DayEntry> GetDayEntry(DateOnly date);

	Task AddHourEntry(DateOnly date, HoursEntry hoursEntry);
}