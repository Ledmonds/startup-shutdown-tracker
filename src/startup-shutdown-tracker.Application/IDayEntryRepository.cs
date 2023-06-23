using startup_shutdown_tracker.Application.Calculators.Day;
using startup_shutdown_tracker.Application.Calculators.Hour;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace startup_shutdown_tracker.Application;

public interface IDayEntryRepository
{
	Task<IReadOnlyCollection<DayEntry>> GetDayEntries();

	Task<DayEntry> GetDayEntry(DateOnly date);

	Task AddHourEntry(DateOnly date, HoursEntry hoursEntry);
}