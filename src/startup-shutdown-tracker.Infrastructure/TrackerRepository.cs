using CsvHelper;
using startup_shutdown_tracker.Application;
using startup_shutdown_tracker.Domain;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace startup_shutdown_tracker.Infrastructure;

public class TrackerRepository : ITrackerRepository
{
	private const string _filepath = @"C:\\startup-shutdown.csv";
	private static readonly CultureInfo _cultureInfo = CultureInfo.CurrentCulture;

	public async Task<Tracker> GetTrackerAsync()
	{
		if (!File.Exists(_filepath))
		{
			return new Tracker();
		}

		using var reader = new StreamReader(_filepath);
		using var csv = new CsvReader(reader, _cultureInfo);
		var trackerEntries = await csv.GetRecordsAsync<TrackerEntry>().ToListAsync();

		return new Tracker() { Entries = trackerEntries, };
	}

	public async Task SaveTrackerAsync(Tracker tracker)
	{
		using var writer = new StreamWriter(_filepath);
		using (var csv = new CsvWriter(writer, _cultureInfo))
		{
			await csv.WriteRecordsAsync(tracker.Entries);
		}
	}
}