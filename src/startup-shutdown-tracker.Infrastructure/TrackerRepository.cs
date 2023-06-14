using startup_shutdown_tracker.Application;
using startup_shutdown_tracker.Domain;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace startup_shutdown_tracker.Infrastructure;

public class TrackerRepository : ITrackerRepository
{
	private const string _delimiter = ",";
	private const string _filepath = @"C:\\startup-shutdown.csv";

	public async Task<Tracker> GetTrackerAsync()
	{
		if (!File.Exists(_filepath))
		{
			return new Tracker();
		}

		var trackerInput = await File.ReadAllLinesAsync(_filepath);
		var entries = trackerInput
			.Select(x =>
			{
				var entry = x.Split(_delimiter);
				return new TrackerEntry()
				{
					Date = DateOnly.Parse(entry[0]),
					StartedAt = !string.IsNullOrEmpty(entry[1])
						? DateTimeOffset.Parse(entry[1])
						: null,
					EndedAt = !string.IsNullOrEmpty(entry[2])
						? DateTimeOffset.Parse(entry[2])
						: null,
				};
			})
			.ToList();

		return new Tracker() { Entries = entries, };
	}

	public async Task SaveTrackerAsync(Tracker tracker)
	{
		var trackerOutput = tracker.Entries
			.Select(x => $"{x.Date}{_delimiter}{x.StartedAt}{_delimiter}{x.EndedAt}")
			.ToList();

		await File.WriteAllLinesAsync(_filepath, trackerOutput);
	}
}