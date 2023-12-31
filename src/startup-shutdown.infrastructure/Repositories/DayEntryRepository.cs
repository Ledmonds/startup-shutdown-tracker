﻿using startup_shutdown.calculation.application;
using startup_shutdown.calculation.application.Calculators.Day;
using startup_shutdown.calculation.application.Calculators.Hour;
using startup_shutdown.tracking.application;
using startup_shutdown.tracking.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace startup_shutdown.infrastructure.Repositories;

public class DayEntryRepository : IDayEntryRepository
{
	private readonly ITrackerRepository _trackerRepository;

	public DayEntryRepository(ITrackerRepository trackerRepository)
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

		var hourEntry = new TrackerEntry()
		{
			Id = Guid.NewGuid(),
			Date = date,
			StartedAt = hoursEntry.StartedAt,
			EndedAt = hoursEntry.EndedAt
		};

		tracker.Entries.Add(hourEntry);

		await _trackerRepository.SaveTrackerAsync(tracker);
	}
}