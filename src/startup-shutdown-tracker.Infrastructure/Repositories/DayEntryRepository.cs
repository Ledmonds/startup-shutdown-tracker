using startup_shutdown_tracker.Application;
using startup_shutdown_tracker.Application.Calculators.Day;
using startup_shutdown_tracker.Application.Calculators.Hour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace startup_shutdown_tracker.Infrastructure.Repositories;

public class DayEntryRepository : IDayEntryRepository
{
    private readonly TrackerRepository _trackerRepository;

    public DayEntryRepository(TrackerRepository trackerRepository)
    {
        _trackerRepository = trackerRepository;
    }

    public async Task<DayEntry> GetDayEntry(DateOnly date)
    {
        var tracker = await _trackerRepository.GetTrackerAsync();

        var hourEntries = tracker.Entries
            .Where(entry => entry.Date == date)
            .Select(entry => new HoursEntry(entry.StartedAt.Value, entry.EndedAt.Value))
            .ToList();

        return new(date, hourEntries);
    }

    public async Task<IReadOnlyCollection<DayEntry>> GetDayEntries()
    {
        var tracker = await _trackerRepository.GetTrackerAsync();

        var dayEntries = tracker.Entries
            .GroupBy(entry => entry.Date)
            .Select(grouping =>
            {
                var hours = grouping
                    .Select(entry => new HoursEntry(entry.StartedAt.Value, entry.EndedAt.Value))
                    .ToList();

                return new DayEntry(grouping.Key, hours);
            })
            .ToArray();

        return dayEntries;
    }

    public async Task AddHourEntry(DateOnly date, HoursEntry hoursEntry)
    {
        var tracker = await _trackerRepository.GetTrackerAsync();

        tracker.Entries.Add(new(date, hoursEntry.StartedAt, hoursEntry.EndedAt));

        await _trackerRepository.SaveTrackerAsync(tracker);
    }
}
